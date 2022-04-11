using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.Entities;

namespace cuentasctacte_web_api.Controllers
{
    public class PedidoDetallesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PedidoDetalles
        public IQueryable<PedidoDetalle> GetPedidoDetalles()
        {
            return db.PedidoDetalles;
        }

        // GET: api/PedidoDetalles/5
        [ResponseType(typeof(PedidoDetalle))]
        public IHttpActionResult GetPedidoDetalle(int id)
        {
            PedidoDetalle pedidoDetalle = db.PedidoDetalles.Find(id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            return Ok(pedidoDetalle);
        }

        // PUT: api/PedidoDetalles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPedidoDetalle(int id, PedidoDetalle pedidoDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedidoDetalle.Id)
            {
                return BadRequest();
            }

            db.Entry(pedidoDetalle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoDetalleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PedidoDetalles
        [ResponseType(typeof(PedidoDetalle))]
        public IHttpActionResult PostPedidoDetalle(PedidoDetalle pedidoDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PedidoDetalles.Add(pedidoDetalle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pedidoDetalle.Id }, pedidoDetalle);
        }

        // DELETE: api/PedidoDetalles/5
        [ResponseType(typeof(PedidoDetalle))]
        public IHttpActionResult DeletePedidoDetalle(int id)
        {
            PedidoDetalle pedidoDetalle = db.PedidoDetalles.Find(id);
            if (pedidoDetalle == null)
            {
                return NotFound();
            }

            db.PedidoDetalles.Remove(pedidoDetalle);
            db.SaveChanges();

            return Ok(pedidoDetalle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidoDetalleExists(int id)
        {
            return db.PedidoDetalles.Count(e => e.Id == id) > 0;
        }
    }
}