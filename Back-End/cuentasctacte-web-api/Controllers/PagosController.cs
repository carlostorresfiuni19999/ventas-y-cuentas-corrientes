﻿using cuentasctacte_web_api.Helpers;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize(Roles = "Administrador,Cajero")]
    public class PagosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpPost]
        [Route("api/Pagos")]
        public IHttpActionResult PostPagos(PagoRequestDTO Cuota)
        {
            if (!ModelState.IsValid) return BadRequest("Formato no valido");
            var Cajero = GetUserLogged.GetUser(db, User.Identity.GetUserId());
            var VencimientoFactura = db.VencimientoFacturas
                .Include(f => f.Factura)
                .Where(f => f.Id == Cuota.IdCuota)
                .FirstOrDefault();

            if (VencimientoFactura.Deleted) return NotFound();

            var Factura = db.VencimientoFacturas
                .Include(f => f.Factura)
                .Where(f => f.Id == Cuota.IdCuota)
                .Select(f => f.Factura)
                .FirstOrDefault();

            if (Factura.Deleted) return NotFound();

            if (Factura == null) return BadRequest("Cuota No valida");
            var Caja = db.Cajas.Include(c => c.Cajero)
                .FirstOrDefault(c => c.IdCajero.Equals(c.IdCajero));
            var Cabecera = new Pago()
            {
                MontoTotal = 0.0,
                IdVencimientoFactura = Cuota.IdCuota,
                IdCajero = Cajero.Id,
                IdCaja = Caja.Id,
                IdCliente = (int)Factura.ClienteId,
                FechaPago = DateTime.Now,
            };

            Cabecera = db.Pagos.Add(Cabecera);

            double MontoTotal = 0.0;

            foreach (var item in Cuota.MetodosPagos)
            {
                var Detalle = new FormasPagos()
                {
                    IdPago = Cabecera.Id,
                    Monto = item.Monto,
                    MetodoPago = item.MetodoPago,
                };
                db.FormasPagos.Add(Detalle);

                MontoTotal += item.Monto;
            }

            Cabecera.MontoTotal = MontoTotal;
            Factura.Saldo -= MontoTotal;
            if (VencimientoFactura.Saldo - MontoTotal < 0)
            {
                return BadRequest("El monto que ingresa es superior al Saldo pendiente");

            }
            VencimientoFactura.Saldo -= MontoTotal;

            
            if(Factura.Saldo == 0 && Factura.Monto == 0)
            {
                Factura.Estado = "PENDIENTE";
            }else if(Factura.Saldo == 0)
            {
                Factura.Estado = "PAGADO";
            } else
            {
                Factura.Estado = "PROCESANDO";
            }

            
            Caja.Saldo += MontoTotal;

            db.Entry(Caja).State = EntityState.Modified;
            db.Entry(Factura).State = EntityState.Modified;
            db.Entry(VencimientoFactura).State = EntityState.Modified;
            db.Entry(Cabecera).State = EntityState.Added;

            try
            {
                db.SaveChanges();
                return Ok("Cobrado Con exito");
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }


        }
        [HttpGet]
        [Route("api/Pagos")]
        [ResponseType(typeof(FullPagoResponseDTO))]
        public IHttpActionResult ViewRecibo(int Id)
        {
            var query = db.Pagos;
            if (query.Find(Id) == null) return NotFound();

            return Ok(query
                .Include(p => p.Caja)
                .Include(p => p.Cajero)
                .Include(p => p.Cliente)
                .Include(p => p.VencimientoFactura)
                .Where(p => !p.Deleted)
                .ToList()
                .ConvertAll(p => MapToFullPagos(p)));
        }

        [HttpGet]
        [Route("api/Pagos")]
        public List<PagoResponseDTO> GetPagos()
        {
            List<PagoResponseDTO> result = db.Pagos
                .Include(p => p.VencimientoFactura)
                .Include(p => p.Cliente)
                .Where(p => !p.Deleted)
                .ToList()
                .ConvertAll(p => new PagoResponseDTO()
                {
                    Id = p.Id,
                    CI = p.Cliente.Documento,
                    FechaCreado = p.FechaPago,
                    MontoTotal = p.MontoTotal,
                    Cliente = p.Cliente.Nombre + " "+p.Cliente.Apellido
                });

            return result;

        }
        [ResponseType(typeof(List<FullPagoResponseDTO>))]
        [HttpGet]
        [Route("api/Pagos")]
        public IHttpActionResult GetPagosByCuota(int IdCuota)
        {
            DbSet<Pago> pagos = db.Pagos;
            var Pago = pagos
                .Include(p => p.Cliente)
                .Include(p => p.Cajero)
                .Include(p => p.VencimientoFactura)
                .Where(p => !p.Deleted)
                .Where(p => p.IdVencimientoFactura == IdCuota);

            
            return Ok( Pago
                .ToList()
                .ConvertAll(p => MapToFullPagos(p)));  
        }
        [HttpDelete]
        [Route("api/Pagos")]
        public IHttpActionResult RemovePago(int Id)
        {
            var Query = db.Pagos
                .Include(p => p.VencimientoFactura)
                .Include(p => p.Caja)
                .Include(p => p.Cajero)
                .Include(p => p.Cliente);

            var Pago = Query.FirstOrDefault(p => p.Id == Id);

            var Caja = db.Cajas.Include(c => c.Cajero)
             .FirstOrDefault(c => c.Id == Pago.IdCaja);
            if (Pago == null) return NotFound();

            //Eliminamos usando LINQ
            db.FormasPagos
                .Where(p => p.IdPago == Id)
                .ForEachAsync(p =>
                {
                    p.Deleted = true;
                    db.Entry(p).State = EntityState.Modified;
                }
                );

            var Vencimiento = Query
                .Where(p => p.Id == Id)
                .Select(p => p.VencimientoFactura)
                .FirstOrDefault();

            Vencimiento.Saldo += Pago.MontoTotal;

            var Factura = Vencimiento.Factura;
            Factura.Saldo += Pago.MontoTotal;

            var Cliente = Factura.Cliente;

            Cliente.Saldo += Pago.MontoTotal;
            Pago.Deleted = true;

            Caja.Saldo -= Pago.MontoTotal;
            //Actualizamos los cambios

            db.Entry(Caja).State = EntityState.Modified;
            db.Entry(Cliente).State = EntityState.Modified;
            db.Entry(Pago).State = EntityState.Modified;
            db.Entry(Factura).State = EntityState.Modified;
            db.Entry(Vencimiento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok("Borrado con exito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        

        private FullPagoResponseDTO MapToFullPagos(Pago Pago)
        {
            if (Pago == null) return null;
            var FormasPagos = db.FormasPagos
                .Where(fp => !fp.Deleted)
                .Where(fp => fp.IdPago == Pago.Id)
                .ToList();

            var result = new FullPagoResponseDTO()
            {
                Id = Pago.Id,
                FechaPago = Pago.FechaPago,
                Cliente = new PersonaResponseDTO()
                {
                    Id = Pago.IdCliente,
                    Nombre = Pago.Cliente.Nombre,
                    Apellido = Pago.Cliente.Apellido,
                    DocumentoTipo = Pago.Cliente.DocumentoTipo,
                    Documento = Pago.Cliente.Documento
                },
                Cajero = new PersonaResponseDTO()
                {
                    Id = Pago.IdCajero,
                    Nombre = Pago.Cajero.Nombre,
                    Apellido = Pago.Cajero.Apellido,
                    DocumentoTipo = Pago.Cajero.DocumentoTipo,
                    Documento = Pago.Cajero.Documento
                },
                MontoTotal = Pago.MontoTotal,
                FormasPagos = FormasPagos
                .ConvertAll(fp => new FormasPagosResponseDTO()
                {
                    Monto = fp.Monto,
                    FormaPago = fp.MetodoPago
                })
            };
            return result;



        }


    }
}
