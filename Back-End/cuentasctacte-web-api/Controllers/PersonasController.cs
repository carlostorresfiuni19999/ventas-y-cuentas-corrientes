using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;
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
    
    public class PersonasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles ="Administrador")]
        [Route("api/Personas/find")]
        [ResponseType(typeof(FullEmpleadoResponseDTO))]
        public IHttpActionResult GetById(int id)
        {
            if (db.Personas.Find(id) == null) return NotFound();

            var result = db.Personas
                .Where(p => !p.Deleted)
                .FirstOrDefault(p => p.Id == id);

            return Ok(MapToFullDTO(result));

        }
        // GET: api/Personas
        [Authorize]
        public List<PersonaResponseDTO> GetPersonas()
        {
            return db.Personas_Tipos_Personas
                .Include(tp => tp.Persona)
                .Include(tp => tp.TipoPersona)
                .Where(tp => tp.TipoPersona.Tipo.Equals("Cliente"))
                .Select(tp => tp.Persona)
                .ToList()
                .ConvertAll(p => new PersonaResponseDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Documento = p.Documento,
                    DocumentoTipo = p.DocumentoTipo,
                    UserName = p.UserName
                });
        }
        [Route("api/Personas/all")]
        [Authorize(Roles = "Administrador")]
        [ResponseType(typeof(List<FullEmpleadoResponseDTO>))]
        public IHttpActionResult GetAllPeople()
        {
            Persona Logged = Helpers.GetUserLogged.GetUser(db, User.Identity.GetUserId());
            return Ok( db.Personas
                .Where(p => !p.Deleted)
                .Where(p => p.Id != Logged.Id)
                .ToList()
                .ConvertAll(p => MapToFullDTO(p)));
        }

            [Authorize]

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
                DocumentoTipo = persona.DocumentoTipo,
                UserName = persona.UserName
            });
        }

 
        // POST: api/Personas
        [Authorize(Roles= "Administrador")]
        [ResponseType(typeof(string))]
        public IHttpActionResult PostPersona(PersonaRequestDTO persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Persona = new Persona()
            {
                Nombre = persona.Nombre,
                Apellido = persona.Apellido,
                Documento = persona.Doc,
                LineaDeCredito = persona.LineaDeCredito,
                Saldo = 0,
                DocumentoTipo = persona.DocumentoTipo,
                Deleted = false,
                Telefono = persona.Telefono,
                UserName = persona.UserName
            };
            var PersonaSaved = db.Personas.Add(Persona);

            foreach(var rol in persona.Roles)
            {
                var Role = db.TipoPersonas
                    .FirstOrDefault(r => r.Tipo.Equals(rol));

                if (Role == null) return BadRequest("Rol no valido");
                var PersonaTipoPersona = new Personas_Tipos_Personas()
                {
                    IdPersona = PersonaSaved.Id,
                    IdTipoPersona = Role.Id,
                    Deleted = false,
                };

                db.Personas_Tipos_Personas.Add(PersonaTipoPersona);
            }
            try
            {
                db.SaveChanges();
                return Ok("Guardado Con exito");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles ="Administrador")]
        [ResponseType(typeof(List<FullEmpleadoResponseDTO>))]
        [Route("api/Personas/Empleados")]
        [HttpGet]
        public IHttpActionResult GetEmpleados(string Role)
        {
            var role = db.TipoPersonas.FirstOrDefault(r => r.Tipo.Equals(Role));
            if(role == null) return NotFound();
            Persona Logged = Helpers.GetUserLogged.GetUser(db, User.Identity.GetUserId());

            var Personas = db.Personas_Tipos_Personas
                .Include(tp => tp.Persona)
                .Include(tp => tp.TipoPersona)
                .Where(tp => tp.TipoPersona.Tipo.Equals(Role))
                .Where(tp => tp.IdPersona != Logged.Id)
                .Select(tp => tp.Persona)
                .ToList()
                .ConvertAll(p => MapToFullDTO(p)) ;
            return Ok(Personas);

        }

       

            // DELETE: api/Personas/5
            [Authorize(Roles = "Administrador")]
        [ResponseType(typeof(Persona))]
        public IHttpActionResult DeletePersona(int id)
        {
            Persona persona = db.Personas
                .Find(id);
            if (persona == null)
            {
                return NotFound();
            }

            persona.Deleted = true;
            db.Entry(persona).State = EntityState.Modified;
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

        private FullEmpleadoResponseDTO MapToFullDTO(Persona p)
        {
            
            return new FullEmpleadoResponseDTO()
            {
                Id = p.Id,
                UserName = p.UserName,
                DocumentoTipo = p.DocumentoTipo,
                Apellido = p.Apellido,
                Nombre = p.Nombre,
                LineaDeCredito = p.LineaDeCredito,
                Saldo = p.Saldo,
                Telefono = p.Telefono,
                Documento = p.Documento,
                Roles = db.Personas_Tipos_Personas
                    .Include(t => t.Persona)
                    .Include(t => t.TipoPersona)
                    .Where(t => t.IdPersona == p.Id)
                    .Select(t => t.TipoPersona.Tipo)
                    .ToList()

            };
        }
    }
}