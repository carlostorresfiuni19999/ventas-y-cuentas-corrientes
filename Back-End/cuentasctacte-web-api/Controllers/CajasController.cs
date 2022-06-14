using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.Entities;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Description;
using cuentasctacte_web_api.Models.DTOs;
using System.Collections.Generic;

namespace cuentasctacte_web_api.Controllers
{
    public class CajasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/Cajas")]
        [Authorize(Roles = "Administrador")]
        [ResponseType(typeof(List<CajaResponseDTO>))]
        public IHttpActionResult GetCajas()
        {
            var query = from caja in db.Cajas
                        join cajero in db.Personas
                        on caja.IdCajero equals cajero.Id
                        where !caja.Deleted
                        select new
                        {
                            IdCaja = caja.Id,
                            Saldo = caja.Saldo,
                            Cajero = cajero.Nombre + " " + cajero.Apellido,
                            UserName = cajero.UserName,
                            Nombre =  caja.NombreCaja
                        };
            return Ok(query.ToList().ConvertAll(t => new CajaResponseDTO()
            {
                Saldo = t.Saldo,
                IdCaja = t.IdCaja,
                Cajero = t.Cajero,
                UserName = t.UserName,
                Nombre = t.Nombre

            }));
        }

        [HttpPost]
        [Route("api/Cajas")]
        [Authorize(Roles = "Administrador")]
        [ResponseType(typeof(string))]
        public IHttpActionResult PostCaja(CajaRequestDTO caja) {
            if (caja.SaldoInicial < 0)
            {
                return BadRequest("No se aceptan saldo Negativos");
            }
            if (!(Helpers.GetRole.HasRole(db, caja.UserName, "Cajero"))) 
                return BadRequest("Rol no valido");

            

            var Persona = from persona in db.Personas
                          where !persona.Deleted
                          && persona.UserName.Equals(caja.UserName)
                          select persona.Id;
            if (db.Cajas
                .Where(c => c.IdCajero == Persona.FirstOrDefault())
                .Count() > 0
                )
                return BadRequest("El usuario " + caja.UserName + " Ya fue asignada a otra caja");

                var Caja = new Caja()
            {
                Saldo = caja.SaldoInicial,
                IdCajero = Persona.FirstOrDefault(),
                NumCaja = db.Cajas.Count(),
                Deleted = false,
                NombreCaja = caja.NombreCaja
            };
            db.Cajas.Add(Caja);

            try
            {
                db.SaveChanges();
                return Ok("Guardado con exito");
            }
            catch
            {
                return BadRequest("Ocurrio un error al ejecutar la transaccion");
            }


        }


          
    }
}