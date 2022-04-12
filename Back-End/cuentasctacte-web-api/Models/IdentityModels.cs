using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace cuentasctacte_web_api.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Tenga en cuenta que authenticationType debe coincidir con el valor definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Agregar reclamaciones de usuario personalizadas aquí
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("VentasYCuentasCorrientes", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.Producto> Productos { get; set; }
        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.TipoPersona> TipoPersonas { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.Persona> Personas { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.Personas_Tipos_Personas> Personas_Tipos_Personas { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.Deposito> Depositos { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Stock> Stocks { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.Pedido> Pedidos { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Modificaciones> Modificaciones { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.PedidoDetalle> PedidoDetalles { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.Factura> Facturas { get; set; }

        public System.Data.Entity.DbSet<cuentasctacte_web_api.Models.Entities.FacturaDetalle> FacturaDetalles { get; set; }
    }
}