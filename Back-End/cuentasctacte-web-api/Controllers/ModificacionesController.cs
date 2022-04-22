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

namespace cuentasctacte_web_api.Controllers
{
    public class ModificacionesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Modificaciones
        public IQueryable<Modificaciones> GetModificaciones()
        {
            return db.Modificaciones;
        }

        // GET: api/Modificaciones/5
        [ResponseType(typeof(Modificaciones))]
        public IHttpActionResult GetModificaciones(int id)
        {
            Modificaciones modificaciones = db.Modificaciones.Find(id);
            if (modificaciones == null)
            {
                return NotFound();
            }

            return Ok(modificaciones);
        }

        // PUT: api/Modificaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutModificaciones(int id, Modificaciones modificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modificaciones.Id)
            {
                return BadRequest();
            }

            db.Entry(modificaciones).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModificacionesExists(id))
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

        // POST: api/Modificaciones
        [ResponseType(typeof(Modificaciones))]
        public IHttpActionResult PostModificaciones(Modificaciones modificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Modificaciones.Add(modificaciones);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = modificaciones.Id }, modificaciones);
        }

        // DELETE: api/Modificaciones/5
        [ResponseType(typeof(Modificaciones))]
        public IHttpActionResult DeleteModificaciones(int id)
        {
            Modificaciones modificaciones = db.Modificaciones.Find(id);
            if (modificaciones == null)
            {
                return NotFound();
            }

            db.Modificaciones.Remove(modificaciones);
            db.SaveChanges();

            return Ok(modificaciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModificacionesExists(int id)
        {
            return db.Modificaciones.Count(e => e.Id == id) > 0;
        }
    }
}