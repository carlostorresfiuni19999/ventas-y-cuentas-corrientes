namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacturaDetalles : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id); ;
            AddForeignKey("FacturaDetalles", "ProductoId", "Productos", "Id");
            AddForeignKey("FacturaDetalles", "FacturaId","Facturas","Id");


        }

        public override void Down()
        {
            DropForeignKey("dbo.FacturaDetalles", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.FacturaDetalles", "FacturaId", "dbo.Facturas");
            DropIndex("dbo.FacturaDetalles", new[] { "ProductoId" });
            DropIndex("dbo.FacturaDetalles", new[] { "FacturaId" });
            DropTable("dbo.FacturaDetalles");
        }
    }
}
