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
            //Verificamos si existe la cabecera del Pago, si no existe, creamos uno
            var query = db.Pagos;
            Pago cabecera;
            if(query.Count() <= 0 || query.Find(pago.IdPago) == null)
            {
                cabecera = new Pago()
                {
                    IdCaja = 0,
                    IdCliente = pago.IdCliente,
                    MontoTotal = 0.0,
                    FechaPago = DateTime.Now,
                };
                cabecera = db.Pagos.Add(cabecera);
            }
            else
            {
                cabecera = query
                    .Include(p => p.Caja)
                    .Include(p => p.Cliente)
                    .FirstOrDefault(p => p.Id == pago.IdPago);
            }



            return BadRequest("NULL");
        }
    }
}
