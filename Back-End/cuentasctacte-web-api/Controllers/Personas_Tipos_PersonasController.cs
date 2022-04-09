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
    public class Personas_Tipos_PersonasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Personas_Tipos_Personas
        public IQueryable<Personas_Tipos_Personas> GetPersonas_Tipos_Personas()
        {
            return db.Personas_Tipos_Personas;
        }

        // GET: api/Personas_Tipos_Personas/5
        [ResponseType(typeof(Personas_Tipos_Personas))]
        public IHttpActionResult GetPersonas_Tipos_Personas(int id)
        {
            Personas_Tipos_Personas personas_Tipos_Personas = db.Personas_Tipos_Personas.Find(id);
            if (personas_Tipos_Personas == null)
            {
                return NotFound();
            }

            return Ok(personas_Tipos_Personas);
        }

        // PUT: api/Personas_Tipos_Personas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersonas_Tipos_Personas(int id, Personas_Tipos_Personas personas_Tipos_Personas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personas_Tipos_Personas.Id)
            {
                return BadRequest();
            }

            db.Entry(personas_Tipos_Personas).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Personas_Tipos_PersonasExists(id))
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

        // POST: api/Personas_Tipos_Personas
        [ResponseType(typeof(Personas_Tipos_Personas))]
        public IHttpActionResult PostPersonas_Tipos_Personas(Personas_Tipos_Personas personas_Tipos_Personas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Personas_Tipos_Personas.Add(personas_Tipos_Personas);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = personas_Tipos_Personas.Id }, personas_Tipos_Personas);
        }

        // DELETE: api/Personas_Tipos_Personas/5
        [ResponseType(typeof(Personas_Tipos_Personas))]
        public IHttpActionResult DeletePersonas_Tipos_Personas(int id)
        {
            Personas_Tipos_Personas personas_Tipos_Personas = db.Personas_Tipos_Personas.Find(id);
            if (personas_Tipos_Personas == null)
            {
                return NotFound();
            }

            db.Personas_Tipos_Personas.Remove(personas_Tipos_Personas);
            db.SaveChanges();

            return Ok(personas_Tipos_Personas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Personas_Tipos_PersonasExists(int id)
        {
            return db.Personas_Tipos_Personas.Count(e => e.Id == id) > 0;
        }
    }
}