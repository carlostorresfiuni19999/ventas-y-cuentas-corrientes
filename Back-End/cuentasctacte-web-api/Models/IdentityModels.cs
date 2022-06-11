using cuentasctacte_web_api.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public DbSet<Producto> Productos { get; set; }
        public DbSet<TipoPersona> TipoPersonas { get; set; }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Personas_Tipos_Personas> Personas_Tipos_Personas { get; set; }

        public DbSet<Deposito> Depositos { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Modificaciones> Modificaciones { get; set; }

        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }

        public DbSet<Factura> Facturas { get; set; }

        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        public DbSet<VencimientoFactura> VencimientoFacturas { get; set; }

        public DbSet<NotaDeCredito> NotasDeCreditos { get; set; }

        public DbSet<NotaDetalle> NotaDetalles { get; set; }

        public DbSet<Caja> Cajas { get; set; }

        public DbSet<Pago> Pagos { get; set; }

        public DbSet<FormasPagos> FormasPagos { get; set; }
    }
}