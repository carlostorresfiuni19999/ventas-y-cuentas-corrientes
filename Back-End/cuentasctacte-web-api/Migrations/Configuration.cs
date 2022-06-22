namespace cuentasctacte_web_api.Migrations
{
    using cuentasctacte_web_api.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<cuentasctacte_web_api.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(cuentasctacte_web_api.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            Load(context);
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }

        private void Load(ApplicationDbContext db)
        {
            UserStore<ApplicationUser> Store = new UserStore<ApplicationUser>(db);
            ApplicationUserManager UserManager = new ApplicationUserManager(Store);
            if (db.Users.Count() == 0)
            {
                var User = new ApplicationUser()
                {
                    UserName = "Admin.Administrador@mail.com",
                    Email = "Admin.Administrador@mail.com"
                };

                UserManager.Create(User, "Admin.Administrador123.com");

                db.Users.Add(User);

                if (db.Roles.Count() == 0)
                {
                    LoadRoles(db);

                    UserManager.AddToRole(User.Id, "Administrador");

                    db.Personas.Add(new Models.Entities.Persona()
                    {
                        Nombre = "Carlos",
                        Apellido = "Cristobal",
                        Telefono = "+595 223 123",
                        Documento = "24323442",
                        DocumentoTipo = "CI",
                        UserName = "Admin.Administrador@mail.com",
                        LineaDeCredito = 38000000,
                        Saldo = 0,
                        Deleted = false

                    });
                    db.TipoPersonas.Add(new Models.Entities.TipoPersona()
                    {
                        Tipo = "Cliente"
                    });
                    db.TipoPersonas.Add(new Models.Entities.TipoPersona()
                    {
                        Tipo = "Vendedor"
                    });
                    db.TipoPersonas.Add(new Models.Entities.TipoPersona()
                    {
                        Tipo = "Administrador"
                    });
                    db.TipoPersonas.Add(new Models.Entities.TipoPersona()
                    {
                        Tipo = "Cajero"
                    });

                    db.Personas_Tipos_Personas.Add(new Models.Entities.Personas_Tipos_Personas()
                    {
                        IdPersona = 1,
                        IdTipoPersona = 2,
                        Deleted = false
                    });


                }
                if (db.Depositos.Count() == 0)
                {
                    db.Depositos.Add(new Models.Entities.Deposito()
                    {
                        NombreDeposito = "Deposito 1 - Materias Primas",
                        DescripcionDeposito = "Almacen de Materias Primas",
                        Deleted = false

                    });
                    db.Depositos.Add(new Models.Entities.Deposito()
                    {
                        NombreDeposito = "Deposito 2 - Materias Defectuosas",
                        DescripcionDeposito = "Almacen de Materias Defectuosas",
                        Deleted = false

                    });
                    db.Depositos.Add(new Models.Entities.Deposito()
                    {
                        NombreDeposito = "Deposito 3 - Pedidos Terminadas",
                        DescripcionDeposito = "Almacen de Pedidos Terminados",
                        Deleted = false

                    });
                }

                if (db.Productos.Count() == 0)
                {
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 2000000,
                        DescripcionProducto = "PC barata para los chicos de la casa",
                        NombreProducto = "Frey PC",
                        CodigoDeBarra = "1240dd04-21",
                        MarcaProducto = "Qualcomm",
                        TieneIva = true,
                        Iva = 200000,
                    });

                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 20000000,
                        DescripcionProducto = "PC Gaming, para los juegos mas pesados",
                        NombreProducto = "La Destructura PC",
                        CodigoDeBarra = "234-532mr4",
                        MarcaProducto = "Intel",
                        TieneIva = true,
                        Iva = 2000000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 4600000,
                        DescripcionProducto = "PC barata para home office",
                        NombreProducto = "Hercules PC",
                        CodigoDeBarra = "1240dd04-21",
                        MarcaProducto = "AMD",
                        TieneIva = true,
                        Iva = 4600000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 53000000,
                        DescripcionProducto = "PC para mineria de criptomonedas",
                        NombreProducto = "Odin PC",
                        CodigoDeBarra = "323d42",
                        MarcaProducto = "Intel",
                        TieneIva = true,
                        Iva = 5300000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 2300000,
                        DescripcionProducto = "PC barata para los chicos de la casa",
                        NombreProducto = "Aquiles",
                        CodigoDeBarra = "284229",
                        MarcaProducto = "Intel",
                        TieneIva = true,
                        Iva = 230000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 1800000,
                        DescripcionProducto = "PC barata para los chicos de la casa",
                        NombreProducto = "Surface GO",
                        CodigoDeBarra = "23400000",
                        MarcaProducto = "Microsoft",
                        TieneIva = true,
                        Iva = 180000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 2000000,
                        DescripcionProducto = "PC barata para los chicos de la casa",
                        NombreProducto = "ChromeBook",
                        CodigoDeBarra = "112345d4",
                        MarcaProducto = "Google",
                        TieneIva = true,
                        Iva = 200000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 3500000,
                        DescripcionProducto = "PC barata para homeOffice",
                        NombreProducto = "Frey Core",
                        CodigoDeBarra = "9rkk9394",
                        MarcaProducto = "Intel",
                        TieneIva = true,
                        Iva = 350000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 2500000,
                        DescripcionProducto = "PC barata para docentes",
                        NombreProducto = "ChromeBook Lite",
                        CodigoDeBarra = "283949",
                        MarcaProducto = "Google",
                        TieneIva = true,
                        Iva = 250000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 4000000,
                        DescripcionProducto = "PC barata para programacion",
                        NombreProducto = "MMk 23",
                        CodigoDeBarra = "939ek99",
                        MarcaProducto = "Dell",
                        TieneIva = true,
                        Iva = 400000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 6400000,
                        DescripcionProducto = "PC optima para programacion",
                        NombreProducto = "Frey PC",
                        CodigoDeBarra = "1240dd04-212",
                        MarcaProducto = "AMD",
                        TieneIva = true,
                        Iva = 640000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 21000000,
                        DescripcionProducto = "PC para edicion de videos y peliculas",
                        NombreProducto = "Macbook M1",
                        CodigoDeBarra = "2i39941",
                        MarcaProducto = "Apple",
                        TieneIva = true,
                        Iva = 2100000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 25000000,
                        DescripcionProducto = "Laptop Gaming",
                        NombreProducto = "Destroyer 33k",
                        CodigoDeBarra = "1345531",
                        MarcaProducto = "HP",
                        TieneIva = true,
                        Iva = 2500000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 2000000,
                        DescripcionProducto = "PC barata para los chicos de la casa",
                        NombreProducto = "Frey PC",
                        CodigoDeBarra = "23311133",
                        MarcaProducto = "Unisoc",
                        TieneIva = true,
                        Iva = 200000,
                    });
                    db.Productos.Add(new Models.Entities.Producto()
                    {
                        Precio = 1850000,
                        DescripcionProducto = "PC barata para los chicos de la casa",
                        NombreProducto = "Frey Lite",
                        CodigoDeBarra = "1222003",
                        MarcaProducto = "Dell",
                        TieneIva = true,
                        Iva = 185000,
                    });

                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 1,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 11,
                        IdDeposito = 3,
                        IdProducto = 2,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 15,
                        IdDeposito = 3,
                        IdProducto = 3,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 20,
                        IdDeposito = 3,
                        IdProducto = 4,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 21,
                        IdDeposito = 3,
                        IdProducto = 5,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 22,
                        IdDeposito = 3,
                        IdProducto = 6,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 19,
                        IdDeposito = 3,
                        IdProducto = 7,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 6,
                        IdDeposito = 3,
                        IdProducto = 8,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 9,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 10,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 11,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 12,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 13,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 14,
                    });
                    db.Stocks.Add(new Stock()
                    {
                        Cantidad = 14,
                        IdDeposito = 3,
                        IdProducto = 15,
                    });

                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        private void LoadRoles(ApplicationDbContext db)
        {
            db.Roles.Add(new IdentityRole()
            {
                Name = "Administrador"
            });

            db.Roles.Add(new IdentityRole()
            {
                Name = "Cliente"
            });
            db.Roles.Add(new IdentityRole()
            {
                Name = "Vendedor"
            });
            db.Roles.Add(new IdentityRole()
            {
                Name = "Cajero"
            });

            db.SaveChanges();
        }
    }
}
