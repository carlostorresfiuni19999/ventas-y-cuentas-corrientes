using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class PagosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/Pagos")]
        public IHttpActionResult PostPago(PagosRequestDTO pago)
        {
            //Verificamos si existe una Orden De Pago para la factura
            var Factura = db.Facturas.Find(pago.IdFactura);

            if (Factura == null) return BadRequest("Factura no encontrada");

            if (Factura.Estado != "PENDIENTE") return BadRequest("Probablemente la factura que hace referencia ya tiene una orden de Pago");
            var Caja = db.Cajas.Find();
            var Cabecera = new Pago()
            {
                IdCliente = pago.IdCliente,
                IdCaja = Caja.Id,
                MontoTotal = 0,
                FechaPago = DateTime.Now
            };

            Cabecera = db.Pagos.Add(Cabecera);


            return BadRequest("NULL");
        }
    }
}
