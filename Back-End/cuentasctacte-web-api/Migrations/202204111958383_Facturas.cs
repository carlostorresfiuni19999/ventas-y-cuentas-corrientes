namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas : DbMigration
    {
        public override void Up()
        {
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.ClienteId)
                .ForeignKey("dbo.Pedidos", t => t.PedidoId)
                .ForeignKey("dbo.Personas", t => t.VendedorId)
                .Index(t => t.PedidoId)
                .Index(t => t.ClienteId)
                .Index(t => t.VendedorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facturas", "VendedorId", "dbo.Personas");
            DropForeignKey("dbo.Facturas", "PedidoId", "dbo.Pedidos");
            DropForeignKey("dbo.Facturas", "ClienteId", "dbo.Personas");
            DropIndex("dbo.Facturas", new[] { "VendedorId" });
            DropIndex("dbo.Facturas", new[] { "ClienteId" });
            DropIndex("dbo.Facturas", new[] { "PedidoId" });
            DropTable("dbo.Facturas");
        }
    }
}
