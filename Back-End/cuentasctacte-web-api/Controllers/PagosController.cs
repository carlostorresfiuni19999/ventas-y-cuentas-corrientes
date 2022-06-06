using cuentasctacte_web_api.Helpers;
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
    [Authorize(Roles = "Cajero")]
    public class PagosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpPost]
        [Route("api/Pagos")]
        public IHttpActionResult PostPagos(PagoRequestDTO Cuota)
        {
            var Cajero = GetUserLogged.GetUser(db, User.Identity.GetUserId());
            var VencimientoFactura = db.VencimientoFacturas
                .Include(f => f.Factura)
                .Where(f => f.Id == Cuota.IdCuota)
                .FirstOrDefault();

            var Factura = db.VencimientoFacturas
                .Include(f => f.Factura)
                .Where(f => f.Id == Cuota.IdCuota)
                .Select(f => f.Factura)
                .FirstOrDefault();

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
                    FormaDePago = item.MetodoPago,
                };
                db.FormasPagos.Add(Detalle);

                MontoTotal += item.Monto;
            }

            Cabecera.MontoTotal = MontoTotal;
            Factura.Saldo -= MontoTotal;
            if (VencimientoFactura.Saldo - MontoTotal <= 0)
            {
                return BadRequest("El monto que ingresa es superior al Saldo pendiente");

            }
            VencimientoFactura.Saldo -= MontoTotal;

            var Cuotas = db.VencimientoFacturas
                .Include(f => f.Factura)
                .Where(f => f.FacturaId == Factura.Id);
            bool Pagado = Cuotas.Count() == Cuotas.Count(c => c.Saldo == 0);

            Factura.Estado = Pagado ? "PAGADO" : "PROCESANDO";
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
        public List<PagoResponseDTO> getPagos()
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
                    MontoTotal = p.MontoTotal

                });

            return result;

        }
        [ResponseType(typeof(FullPagoResponseDTO))]
        [HttpGet]
        [Route("api/Pagos")]
        public IHttpActionResult GetPago(int Id)
        {
            var Pago = db
                .Pagos
                .Include(p => p.Cliente)
                .Include(p => p.Cajero)
                .Where(p => !p.Deleted)
                .FirstOrDefault(p => p.Id == Id);

            if (Pago == null) return NotFound();
            var FormasPagos = db.FormasPagos
                .Include(fp => fp.Pago)
                .Where(fp => !fp.Deleted)
                .Where(fp => fp.IdPago == Id);

            var result = new FullPagoResponseDTO()
            {
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
                FormasPagos = FormasPagos.ToList()
                .ConvertAll(fp => new FormasPagosResponseDTO()
                {
                    Monto = fp.Monto,
                    FormaPago = fp.FormaDePago
                })


            };
            return Ok(result);
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
        /* TO-DO
        [HttpPut]
        [Route("api/Pagos")]
        public IHttpActionResult EditPago(int Id, PagoRequestDTO pago)
        {
              var Pago = db.Pagos
                .Include(p => p.Caja)
                .Include(p => p.VencimientoFactura)
                .Include(p => p.Cliente)
                .Include(p => p.Cajero)
                .FirstOrDefault(p => p.Id == Id);

            if (Pago == null) return NotFound();

            //Eliminamos los detalles;
            db.FormasPagos
                .Where(f => !f.Deleted)
                .Where(f => f.IdPago == Id)
                .ForEachAsync(f => db.FormasPagos.Remove(f));

            var Cliente = Pago.Cliente;
            var Factura = db.VencimientoFacturas
                .Include(v => v.Factura)
                .Where(v => !v.Deleted)
                .Where(v => v.Id == Pago.IdVencimientoFactura)
                .Select(v => v.Factura)
                .FirstOrDefault();

            var VencimientoFactura = Pago.VencimientoFactura;

            //Cargamos los nuevos detalles
            double MontoTotal = pago.MetodosPagos.Sum(p => p.Monto);
            pago.MetodosPagos.ForEach(p =>
            {
                db.FormasPagos.Add(new FormasPagos()
                {
                    IdPago = Id,
                    FormaDePago = p.MetodoPago,
                    Monto = p.Monto
                });
            });

            Cliente.Saldo += (MontoTotal - Pago.MontoTotal);
            Factura.Saldo += (MontoTotal - Pago.MontoTotal);
            VencimientoFactura.Saldo += (MontoTotal - Pago.MontoTotal);

            Factura.Estado = db.VencimientoFacturas
                .Where(v => v.Deleted)
                .All(v => v.Saldo == 0) ? "PAGADO" : "PROCESANDO";

            var Cajero = GetUserLogged.GetUser(db, User.Identity.GetUserId());
           
            var Caja = db.Cajas.Include(c => c.Cajero)
                .FirstOrDefault(c => c.IdCajero == Pago.IdCajero);

            var CajaEdited = db.Cajas
                .Include(c => c.Cajero)
                .FirstOrDefault(c => c.IdCajero == Cajero.Id);

            Caja.Saldo = 

            var PagoEdited = new Pago()
            {

                IdCliente = Pago.IdCliente,
                MontoTotal = MontoTotal,
                FechaPago = DateTime.Now,
                IdCaja = Caja.Id,
                IdVencimientoFactura = pago.IdCuota

            };
            
            var VencimientoEdited = db.VencimientoFacturas
                .Include(f => f.Factura)
                .FirstOrDefault(f => f.Id == pago.IdCuota);
            if (VencimientoEdited.Saldo - MontoTotal <= 0) return BadRequest("El monto que ingresa supera al saldo pendiente");
            db.Entry(VencimientoEdited).State = EntityState.Modified;
            db.Entry(Factura).State = EntityState.Modified;
            db.Entry(Cliente).State = EntityState.Modified;
            db.Entry(VencimientoFactura).State = EntityState.Modified;
            db.Entry(PagoEdited).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok("Editado con exito :D");
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }      
           
        }
        */

    }
}
