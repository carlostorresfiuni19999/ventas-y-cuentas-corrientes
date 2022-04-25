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
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;

namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    public class PersonasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Personas
        public List<PersonaResponseDTO> GetPersonas()
        {
            return db.Personas
                .Where(p => !p.Deleted)
                .ToList()
                .ConvertAll(p => new PersonaResponseDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Documento = p.Documento,
                    DocumentoTipo = p.DocumentoTipo
                });
        }

        // GET: api/Personas/8848584
        [ResponseType(typeof(PersonaResponseDTO))]
        public IHttpActionResult GetPersona(string doc)
        {
            var persona = db.Personas
                .Where(p => !p.Deleted)
                .FirstOrDefault(p => p.Documento.Equals(doc));
            if (persona == null)
            {
                return NotFound();
            }

            return Ok(new PersonaResponseDTO
            {
                Id = persona.Id,
                Nombre = persona.Nombre,
                Apellido = persona.Apellido,
                Documento = persona.Documento,
                DocumentoTipo = persona.DocumentoTipo
            });
        }

        // PUT: api/Personas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersona(int id, Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != persona.Id)
            {
                return BadRequest();
            }

            db.Entry(persona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(id))
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

        // POST: api/Personas
        [ResponseType(typeof(Persona))]
        public IHttpActionResult PostPersona(Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Personas.Add(persona);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = persona.Id }, persona);
        }

        // DELETE: api/Personas/5
        [ResponseType(typeof(Persona))]
        public IHttpActionResult DeletePersona(int id)
        {
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return NotFound();
            }

            db.Personas.Remove(persona);
            db.SaveChanges();

            return Ok(persona);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonaExists(int id)
        {
            return db.Personas.Count(e => e.Id == id) > 0;
        }
    }
}