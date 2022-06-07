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
    public class FacturaDetallesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FacturaDetalles
        public IQueryable<FacturaDetalle> GetFacturaDetalles()
        {
            return db.FacturaDetalles;
        }

        // GET: api/FacturaDetalles/5
        [ResponseType(typeof(FacturaDetalle))]
        public IHttpActionResult GetFacturaDetalle(int id)
        {
            FacturaDetalle facturaDetalle = db.FacturaDetalles.Find(id);
            if (facturaDetalle == null || facturaDetalle.Deleted)
            {
                return NotFound();
            }

            return Ok(facturaDetalle);
        }

        // PUT: api/FacturaDetalles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFacturaDetalle(int id, FacturaDetalle facturaDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facturaDetalle.Id)
            {
                return BadRequest();
            }

            db.Entry(facturaDetalle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaDetalleExists(id))
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

        // POST: api/FacturaDetalles
        [ResponseType(typeof(FacturaDetalle))]
        public IHttpActionResult PostFacturaDetalle(FacturaDetalle facturaDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FacturaDetalles.Add(facturaDetalle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = facturaDetalle.Id }, facturaDetalle);
        }

        // DELETE: api/FacturaDetalles/5
        [ResponseType(typeof(FacturaDetalle))]
        public IHttpActionResult DeleteFacturaDetalle(int id)
        {
            FacturaDetalle facturaDetalle = db.FacturaDetalles.Find(id);
            if (facturaDetalle == null)
            {
                return NotFound();
            }

            db.FacturaDetalles.Remove(facturaDetalle);
            db.SaveChanges();

            return Ok(facturaDetalle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FacturaDetalleExists(int id)
        {
            return db.FacturaDetalles.Count(e => e.Id == id) > 0;
        }
    }
}