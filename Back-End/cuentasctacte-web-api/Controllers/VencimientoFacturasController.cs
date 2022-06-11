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
    public class VencimientoFacturasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/VencimientoFacturas
        public IQueryable<VencimientoFactura> GetVencimientoFacturas()
        {
            return db.VencimientoFacturas;
        }

        // GET: api/VencimientoFacturas/5
        [Authorize]
        [ResponseType(typeof(VencimientoFactura))]
        public IHttpActionResult GetVencimientoFactura(int id)
        {
            VencimientoFactura vencimientoFactura = db.VencimientoFacturas.Find(id);
            if (vencimientoFactura == null || vencimientoFactura.Deleted)
            {
                return NotFound();
            }

            return Ok(vencimientoFactura);
        }

        // PUT: api/VencimientoFacturas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVencimientoFactura(int id, VencimientoFactura vencimientoFactura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vencimientoFactura.Id)
            {
                return BadRequest();
            }

            db.Entry(vencimientoFactura).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VencimientoFacturaExists(id))
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

        // POST: api/VencimientoFacturas
        [ResponseType(typeof(VencimientoFactura))]
        public IHttpActionResult PostVencimientoFactura(VencimientoFactura vencimientoFactura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VencimientoFacturas.Add(vencimientoFactura);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vencimientoFactura.Id }, vencimientoFactura);
        }

        // DELETE: api/VencimientoFacturas/5
        [ResponseType(typeof(VencimientoFactura))]
        public IHttpActionResult DeleteVencimientoFactura(int id)
        {
            VencimientoFactura vencimientoFactura = db.VencimientoFacturas.Find(id);
            if (vencimientoFactura == null)
            {
                return NotFound();
            }

            db.VencimientoFacturas.Remove(vencimientoFactura);
            db.SaveChanges();

            return Ok(vencimientoFactura);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VencimientoFacturaExists(int id)
        {
            return db.VencimientoFacturas.Count(e => e.Id == id) > 0;
        }
    }
}