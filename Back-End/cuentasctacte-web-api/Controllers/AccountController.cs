using cuentasctacte_web_api.Helpers;
using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.DTOs;
using cuentasctacte_web_api.Models.Entities;
using cuentasctacte_web_api.Providers;
using cuentasctacte_web_api.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Linq;
namespace cuentasctacte_web_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        // POST api/Account/SetPassword
        [HttpPut]
        [Route("SetPassword")]
        [Authorize(Roles = "Administrador")]
        public async Task<IHttpActionResult> SetPassword(string UserName, SetPasswordBindingModel model)
        {

            var user = db.Users.FirstOrDefault(u => u.UserName.Equals(UserName));
            
            if (user == null) return BadRequest("Usuario no valido");
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);

            db.Entry(user).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                return Ok("Modificado con exito");
            }
            catch
            {
                return BadRequest("Error en la transaccion");
            }

            
        }

       
      
        // POST api/Account/Register
        [Authorize(Roles ="Administrador")]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(PersonaRequestDTO model)
        {
            

            var user = new ApplicationUser() { UserName = model.UserName, Email = model.UserName };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
       
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            foreach (var rol in model.Roles)
            {
                UserManager.AddToRole(user.Id, rol);
            }

            
            // AddPersonas
            var Persona = new Persona()
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Documento = model.Doc,
                LineaDeCredito = model.LineaDeCredito,
                Saldo = 0,
                DocumentoTipo = model.DocumentoTipo,
                Deleted = false,
                Telefono = model.Telefono,
                UserName = model.UserName
            };
            var PersonaSaved = db.Personas.Add(Persona);

            foreach (var rol in model.Roles)
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
            db.SaveChanges();
            return Ok();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult EditPersonaAsync(string username, PersonaUpdateRequestDTO persona)
        {
            Persona Persona = db.Personas
                .Where(p => !p.Deleted)
                .Where(p => p.UserName.Equals(username))
                .FirstOrDefault();

            if (Persona == null) return (BadRequest("Persona no encontrada "));
            var userManager = db.Users.FirstOrDefault(u => u.UserName.Equals(username));

            if (userManager == null) return (NotFound());

            //Verificamos si hay un cambio en la tabla de Personas

            if (!(Persona.LineaDeCredito == persona.LineaDeCredito &&
                Persona.UserName == persona.UserName
                && Persona.Documento == persona.Doc
                && Persona.DocumentoTipo == persona.DocumentoTipo
                && Persona.Telefono == persona.Telefono
                && Persona.Nombre == persona.Nombre
                && Persona.Apellido == persona.Apellido))
            {
                Persona.LineaDeCredito = persona.LineaDeCredito;
                Persona.UserName = persona.UserName;
                Persona.Documento = persona.Doc;
                Persona.DocumentoTipo = persona.DocumentoTipo;
                Persona.Telefono = persona.Telefono;
                Persona.Nombre = persona.Nombre;
                Persona.Apellido = persona.Apellido;

                //Actualizamos



            }
            //Actualizamos primero la Persona en la bd



            //Borramos todos los roles que tenia

            var query = db.Personas_Tipos_Personas
                .Include(ptp => ptp.Persona)
                .Include(ptp => ptp.TipoPersona)
                .Where(ptp => !ptp.Deleted)
                .Where(ptp => ptp.IdPersona == Persona.Id);

            foreach (var ptp in query)
            {
                UserManager.RemoveFromRole(userManager.Id, ptp.TipoPersona.Tipo);

            }

            foreach (var r in persona.Roles)
            {
                UserManager.AddToRole(userManager.Id, r);
            }

            foreach (var ptp in query)
            {
                db.Personas_Tipos_Personas.Remove(ptp);
            }
            //Cargamos los nuevos roles
            persona.Roles.ForEach(r => {
                var roldb = new Personas_Tipos_Personas
                {
                    IdPersona = Persona.Id,
                    IdTipoPersona = db.TipoPersonas
                        .FirstOrDefault(tp => tp.Tipo.Equals(r)).Id,
                    Deleted = false
                };

                db.Personas_Tipos_Personas.Add(roldb);
            }
            );

            userManager.UserName = persona.UserName;
            userManager.Email = persona.UserName;
            db.Entry(userManager).State = EntityState.Modified;
            db.Entry(Persona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return (Ok("Editado con exito"));
            }
            catch
            {
                return (BadRequest("Ocurrio un error al ejecutar la transaccion"));
            }

        }
       
        
        [HttpGet]
        [Authorize]
        [Route("HasRole")]
        public bool HasRole(string email, string role)
        {
            return GetRole.HasRole(db, email, role);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No hay disponibles errores ModelState para enviar, por lo que simplemente devuelva un BadRequest vacío.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits debe ser uniformemente divisible por 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
