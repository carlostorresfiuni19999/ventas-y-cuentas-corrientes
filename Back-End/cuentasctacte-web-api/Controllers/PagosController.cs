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

namespace cuentasctacte_web_api.Controllers
{
    [Authorize(Roles = "Cajero")]
    public class PagosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/Pagos/OrdenDeCobro")]
        public IHttpActionResult CargarOrdenDeCobro(int FacturaId)
        {
            //Verificamos si existe una Orden De Pago para la factura
            var Factura = db.Facturas
                .Include(f => f.Pedido)
                .Include(f => f.Cliente)
                .Where(f => !f.Deleted)
                .FirstOrDefault(f => f.Id == FacturaId);

            if (Factura == null) return BadRequest("Factura no encontrada");

            if (Factura.Estado != "PENDIENTE") return BadRequest("Probablemente la factura que hace referencia ya tiene una orden de cobro");
            var Cajero = GetUserLogged.GetUser(db, User.Identity.GetUserId());
            var Caja = db.Cajas
                .Include(c => c.Cajero)
                .Where(c => !c.Deleted)
                .FirstOrDefault(c => c.IdCajero == Cajero.Id);
            if (Caja == null) return BadRequest("Caja no valida");

            Factura.Estado = "PROCESANDO";
            db.Entry(Factura).State = EntityState.Modified;
            var Cabecera = new Pago()
            {
                IdCliente = (int)Factura.ClienteId,
                IdCaja = Caja.Id,
                MontoTotal = 0,
                FechaPago = DateTime.Now,
                IdCajero = Cajero.Id
            };

            Cabecera = db.Pagos.Add(Cabecera);

            //Cargando Los detalles de la Orden De Pagos

            var cuotas = db.VencimientoFacturas
                .Include(c => c.Factura)
                .Where(c => !c.Deleted);

            foreach (var item in cuotas)
            {
                var Detalle = new PagosDetalle()
                {
                    Monto = 0.0,
                    Id = item.Id,
                    IdPago = Cabecera.Id
                };
                db.PagosDetalles.Add(Detalle);
            }
            try
            {
                db.SaveChanges();
                return Ok("Orden de Cobro Cargada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("Pagos/OrdenDeCobro")]
        public List<PagoResponseDTO> OrdenesDePagos()
        {
            var pagos = db.Pagos
                .Where(p => !p.Deleted)
                .ToList()
                .ConvertAll(p => new PagoResponseDTO()
                {
                    FechaCreado = p.FechaPago,
                    MontoTotal = p.MontoTotal,
                    Cliente = p.Cliente.Nombre + " " + p.Cliente.Apellido,
                    CI = p.Cliente.Documento

                }); ;

            return pagos;
        }
    }
}
