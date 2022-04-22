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
    public class NotasDeCreditosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/NotasDeCreditos
        public IQueryable<NotaDeCredito> GetNotaDeCreditoes()
        {
            return db.NotasDeCreditos;
        }

        // GET: api/NotasDeCreditos/5
        [ResponseType(typeof(NotaDeCredito))]
        public IHttpActionResult GetNotaDeCredito(int id)
        {
            NotaDeCredito notaDeCredito = db.NotasDeCreditos.Find(id);
            if (notaDeCredito == null)
            {
                return NotFound();
            }

            return Ok(notaDeCredito);
        }

        // PUT: api/NotasDeCreditos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotaDeCredito(int id, NotaDeCredito notaDeCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notaDeCredito.Id)
            {
                return BadRequest();
            }

            db.Entry(notaDeCredito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaDeCreditoExists(id))
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

        // POST: api/NotasDeCreditos
        [ResponseType(typeof(NotaDeCredito))]
        public IHttpActionResult PostNotaDeCredito(NotaDeCredito notaDeCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NotasDeCreditos.Add(notaDeCredito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notaDeCredito.Id }, notaDeCredito);
        }

        // DELETE: api/NotasDeCreditos/5
        [ResponseType(typeof(NotaDeCredito))]
        public IHttpActionResult DeleteNotaDeCredito(int id)
        {
            NotaDeCredito notaDeCredito = db.NotasDeCreditos.Find(id);
            if (notaDeCredito == null)
            {
                return NotFound();
            }

            db.NotasDeCreditos.Remove(notaDeCredito);
            db.SaveChanges();

            return Ok(notaDeCredito);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotaDeCreditoExists(int id)
        {
            return db.NotasDeCreditos.Count(e => e.Id == id) > 0;
        }
    }
}