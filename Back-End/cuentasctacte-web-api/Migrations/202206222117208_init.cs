namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cajas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumCaja = c.Int(nullable: false),
                        NombreCaja = c.String(),
                        IdCajero = c.Int(nullable: false),
                        Saldo = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.IdCajero, cascadeDelete: false)
                .Index(t => t.IdCajero);
            
            CreateTable(
                "dbo.Personas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Telefono = c.String(),
                        UserName = c.String(),
                        DocumentoTipo = c.String(),
                        Documento = c.String(),
                        LineaDeCredito = c.Double(nullable: false),
                        Saldo = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Depositos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreDeposito = c.String(),
                        DescripcionDeposito = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FacturaDetalles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacturaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Iva = c.Double(nullable: false),
                        PrecioUnitario = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facturas", t => t.FacturaId, cascadeDelete: false)
                .ForeignKey("dbo.Productos", t => t.ProductoId, cascadeDelete: false)
                .Index(t => t.FacturaId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Facturas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Iva = c.Double(nullable: false),
                        CondicionVenta = c.String(),
                        Estado = c.String(),
                        Monto = c.Double(nullable: false),
                        Saldo = c.Double(nullable: false),
                        FechaFactura = c.DateTime(nullable: false),
                        PedidoId = c.Int(),
                        ClienteId = c.Int(),
                        VendedorId = c.Int(),
                        CantidadCuotas = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.ClienteId)
                .ForeignKey("dbo.Pedidos", t => t.PedidoId)
                .ForeignKey("dbo.Personas", t => t.VendedorId)
                .Index(t => t.PedidoId)
                .Index(t => t.ClienteId)
                .Index(t => t.VendedorId);
            
            CreateTable(
                "dbo.Pedidos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PedidoDescripcion = c.String(),
                        NumeroPedido = c.Int(nullable: false),
                        CondicionVenta = c.String(),
                        Estado = c.String(),
                        FechaPedido = c.DateTime(nullable: false),
                        IdCliente = c.Int(),
                        IdVendedor = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.IdCliente)
                .ForeignKey("dbo.Personas", t => t.IdVendedor)
                .Index(t => t.IdCliente)
                .Index(t => t.IdVendedor);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreProducto = c.String(),
                        DescripcionProducto = c.String(),
                        MarcaProducto = c.String(),
                        CodigoDeBarra = c.String(),
                        TieneIva = c.Boolean(nullable: false),
                        Precio = c.Double(nullable: false),
                        Iva = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FormasPagos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MetodoPago = c.String(),
                        Monto = c.Double(nullable: false),
                        IdPago = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pagos", t => t.IdPago, cascadeDelete: false)
                .Index(t => t.IdPago);
            
            CreateTable(
                "dbo.Pagos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MontoTotal = c.Double(nullable: false),
                        FechaPago = c.DateTime(nullable: false),
                        IdVencimientoFactura = c.Int(nullable: false),
                        IdCliente = c.Int(nullable: false),
                        IdCaja = c.Int(nullable: false),
                        IdCajero = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cajas", t => t.IdCaja, cascadeDelete: false)
                .ForeignKey("dbo.Personas", t => t.IdCajero, cascadeDelete: false)
                .ForeignKey("dbo.Personas", t => t.IdCliente, cascadeDelete: false)
                .ForeignKey("dbo.VencimientoFacturas", t => t.IdVencimientoFactura, cascadeDelete: false)
                .Index(t => t.IdVencimientoFactura)
                .Index(t => t.IdCliente)
                .Index(t => t.IdCaja)
                .Index(t => t.IdCajero);
            
            CreateTable(
                "dbo.VencimientoFacturas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacturaId = c.Int(nullable: false),
                        FechaVencimiento = c.DateTime(nullable: false),
                        Monto = c.Double(nullable: false),
                        Saldo = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facturas", t => t.FacturaId, cascadeDelete: false)
                .Index(t => t.FacturaId);
            
            CreateTable(
                "dbo.Modificaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModificadoPor = c.String(),
                        IdPersona = c.Int(nullable: false),
                        Data = c.String(),
                        ModificacionTipo = c.String(),
                        FechaModificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotaDetalles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        NotaId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Precio = c.Double(nullable: false),
                        Iva = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotasDeCreditos", t => t.NotaId, cascadeDelete: false)
                .ForeignKey("dbo.Productos", t => t.ProductoId, cascadeDelete: false)
                .Index(t => t.ProductoId)
                .Index(t => t.NotaId);
            
            CreateTable(
                "dbo.NotasDeCreditos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        FacturaId = c.Int(),
                        NotaDescripcion = c.String(),
                        FechaElaboracion = c.DateTime(nullable: false),
                        Monto = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.ClienteId, cascadeDelete: false)
                .ForeignKey("dbo.Facturas", t => t.FacturaId)
                .Index(t => t.ClienteId)
                .Index(t => t.FacturaId);
            
            CreateTable(
                "dbo.PedidoDetalles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CantidadProducto = c.Int(nullable: false),
                        CantidadFacturada = c.Int(nullable: false),
                        PrecioUnitario = c.Double(nullable: false),
                        IdProducto = c.Int(nullable: false),
                        IdPedido = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pedidos", t => t.IdPedido, cascadeDelete: false)
                .ForeignKey("dbo.Productos", t => t.IdProducto, cascadeDelete: false)
                .Index(t => t.IdProducto)
                .Index(t => t.IdPedido);
            
            CreateTable(
                "dbo.Personas_Tipos_Personas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdPersona = c.Int(),
                        IdTipoPersona = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.IdPersona)
                .ForeignKey("dbo.TipoPersonas", t => t.IdTipoPersona)
                .Index(t => t.IdPersona)
                .Index(t => t.IdTipoPersona);
            
            CreateTable(
                "dbo.TipoPersonas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        IdProducto = c.Int(),
                        IdDeposito = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depositos", t => t.IdDeposito)
                .ForeignKey("dbo.Productos", t => t.IdProducto)
                .Index(t => t.IdProducto)
                .Index(t => t.IdDeposito);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stocks", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.Stocks", "IdDeposito", "dbo.Depositos");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Personas_Tipos_Personas", "IdTipoPersona", "dbo.TipoPersonas");
            DropForeignKey("dbo.Personas_Tipos_Personas", "IdPersona", "dbo.Personas");
            DropForeignKey("dbo.PedidoDetalles", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.PedidoDetalles", "IdPedido", "dbo.Pedidos");
            DropForeignKey("dbo.NotaDetalles", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.NotaDetalles", "NotaId", "dbo.NotasDeCreditos");
            DropForeignKey("dbo.NotasDeCreditos", "FacturaId", "dbo.Facturas");
            DropForeignKey("dbo.NotasDeCreditos", "ClienteId", "dbo.Personas");
            DropForeignKey("dbo.FormasPagos", "IdPago", "dbo.Pagos");
            DropForeignKey("dbo.Pagos", "IdVencimientoFactura", "dbo.VencimientoFacturas");
            DropForeignKey("dbo.VencimientoFacturas", "FacturaId", "dbo.Facturas");
            DropForeignKey("dbo.Pagos", "IdCliente", "dbo.Personas");
            DropForeignKey("dbo.Pagos", "IdCajero", "dbo.Personas");
            DropForeignKey("dbo.Pagos", "IdCaja", "dbo.Cajas");
            DropForeignKey("dbo.FacturaDetalles", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.FacturaDetalles", "FacturaId", "dbo.Facturas");
            DropForeignKey("dbo.Facturas", "VendedorId", "dbo.Personas");
            DropForeignKey("dbo.Facturas", "PedidoId", "dbo.Pedidos");
            DropForeignKey("dbo.Pedidos", "IdVendedor", "dbo.Personas");
            DropForeignKey("dbo.Pedidos", "IdCliente", "dbo.Personas");
            DropForeignKey("dbo.Facturas", "ClienteId", "dbo.Personas");
            DropForeignKey("dbo.Cajas", "IdCajero", "dbo.Personas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Stocks", new[] { "IdDeposito" });
            DropIndex("dbo.Stocks", new[] { "IdProducto" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "IdTipoPersona" });
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "IdPersona" });
            DropIndex("dbo.PedidoDetalles", new[] { "IdPedido" });
            DropIndex("dbo.PedidoDetalles", new[] { "IdProducto" });
            DropIndex("dbo.NotasDeCreditos", new[] { "FacturaId" });
            DropIndex("dbo.NotasDeCreditos", new[] { "ClienteId" });
            DropIndex("dbo.NotaDetalles", new[] { "NotaId" });
            DropIndex("dbo.NotaDetalles", new[] { "ProductoId" });
            DropIndex("dbo.VencimientoFacturas", new[] { "FacturaId" });
            DropIndex("dbo.Pagos", new[] { "IdCajero" });
            DropIndex("dbo.Pagos", new[] { "IdCaja" });
            DropIndex("dbo.Pagos", new[] { "IdCliente" });
            DropIndex("dbo.Pagos", new[] { "IdVencimientoFactura" });
            DropIndex("dbo.FormasPagos", new[] { "IdPago" });
            DropIndex("dbo.Pedidos", new[] { "IdVendedor" });
            DropIndex("dbo.Pedidos", new[] { "IdCliente" });
            DropIndex("dbo.Facturas", new[] { "VendedorId" });
            DropIndex("dbo.Facturas", new[] { "ClienteId" });
            DropIndex("dbo.Facturas", new[] { "PedidoId" });
            DropIndex("dbo.FacturaDetalles", new[] { "ProductoId" });
            DropIndex("dbo.FacturaDetalles", new[] { "FacturaId" });
            DropIndex("dbo.Cajas", new[] { "IdCajero" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Stocks");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TipoPersonas");
            DropTable("dbo.Personas_Tipos_Personas");
            DropTable("dbo.PedidoDetalles");
            DropTable("dbo.NotasDeCreditos");
            DropTable("dbo.NotaDetalles");
            DropTable("dbo.Modificaciones");
            DropTable("dbo.VencimientoFacturas");
            DropTable("dbo.Pagos");
            DropTable("dbo.FormasPagos");
            DropTable("dbo.Productos");
            DropTable("dbo.Pedidos");
            DropTable("dbo.Facturas");
            DropTable("dbo.FacturaDetalles");
            DropTable("dbo.Depositos");
            DropTable("dbo.Personas");
            DropTable("dbo.Cajas");
        }
    }
}
