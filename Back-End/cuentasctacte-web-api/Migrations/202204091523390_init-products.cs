namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initproducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                 "dbo.Productos",
                 c => new
                 {
                     Id = c.Int(nullable: false, identity: true),
                     NombreProducto = c.String(),
                     DescripcionProducto = c.String(),
                     MarcaProducto = c.String(),
                     CodigoDeBarra = c.String(),
                     TieneIva = c.Boolean(),
                     Precio = c.Double(),
                     Iva = c.Double(),
                     Deleted = c.Boolean()
                 })
                 .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Productos");
        }
    }
}
