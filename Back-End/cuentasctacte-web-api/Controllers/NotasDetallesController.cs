using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace cuentasctacte_web_api.Controllers
{
    public class NotasDetallesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/NotasDetalles
        public IQueryable<NotaDetalle> GetNotaDetalles()
        {
            return db.NotaDetalles;
        }

        // GET: api/NotasDetalles/5
        [ResponseType(typeof(NotaDetalle))]
        public IHttpActionResult GetNotaDetalle(int id)
        {
            NotaDetalle notaDetalle = db.NotaDetalles.Find(id);
            if (notaDetalle == null)
            {
                return NotFound();
            }

            return Ok(notaDetalle);
        }

        // PUT: api/NotasDetalles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotaDetalle(int id, NotaDetalle notaDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notaDetalle.Id)
            {
                return BadRequest();
            }

            db.Entry(notaDetalle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaDetalleExists(id))
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

        // POST: api/NotasDetalles
        [ResponseType(typeof(NotaDetalle))]
        public IHttpActionResult PostNotaDetalle(NotaDetalle notaDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NotaDetalles.Add(notaDetalle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notaDetalle.Id }, notaDetalle);
        }

        // DELETE: api/NotasDetalles/5
        [ResponseType(typeof(NotaDetalle))]
        public IHttpActionResult DeleteNotaDetalle(int id)
        {
            NotaDetalle notaDetalle = db.NotaDetalles.Find(id);
            if (notaDetalle == null)
            {
                return NotFound();
            }

            db.NotaDetalles.Remove(notaDetalle);
            db.SaveChanges();

            return Ok(notaDetalle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotaDetalleExists(int id)
        {
            return db.NotaDetalles.Count(e => e.Id == id) > 0;
        }
    }
}