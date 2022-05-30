using cuentasctacte_web_api.Helpers;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize(Roles = "cajero")]
    public class PagosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/Pagos/OrdenDeCobro")]
        public IHttpActionResult CargarOrdenDeCobro(PagosRequestDTO pago)
        {
            //Verificamos si existe una Orden De Pago para la factura
            var Factura = db.Facturas.Find(pago.IdFactura);

            if (Factura == null) return BadRequest("Factura no encontrada");

            if (Factura.Estado != "PENDIENTE") return BadRequest("Probablemente la factura que hace referencia ya tiene una orden de cobro");
            var Cajero = GetUserLogged.GetUser(db, User.Identity.GetUserId());
            var Caja = db.Cajas
                .Include(c => c.Cajero)
                .Where(c => !c.Deleted)
                .FirstOrDefault(c => c.IdCajero == Cajero.Id);
            if (Caja == null) return BadRequest("Caja no valida");


            var Cabecera = new Pago()
            {
                IdCliente = pago.IdCliente,
                IdCaja = Caja.Id,
                MontoTotal = 0,
                FechaPago = DateTime.Now,
                IdCajero = Cajero.Id
            };

            Cabecera = db.Pagos.Add(Cabecera);

            //Cargando Los detalles de la Orden De Pagos

            var cuotas = db.VencimientoFacturas
                .Where(c => !c.Deleted)
                .Include(c => c.FacturaId == pago.IdFactura);

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

        public IHttpActionResult 
    }
}
