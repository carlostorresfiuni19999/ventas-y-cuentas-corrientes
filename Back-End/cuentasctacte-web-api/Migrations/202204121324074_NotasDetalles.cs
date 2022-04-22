namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotasDetalles : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id); ;
            AddForeignKey("NotaDetalles", "ProductoId", "Productos", "Id");
            AddForeignKey("NotaDetalles", "NotaId", "NotasDeCreditos", "Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaDetalles", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.NotaDetalles", "NotaId", "dbo.NotasDeCreditos");
            DropIndex("dbo.NotaDetalles", new[] { "NotaId" });
            DropIndex("dbo.NotaDetalles", new[] { "ProductoId" });
            DropTable("dbo.NotaDetalles");
        }
    }
}
