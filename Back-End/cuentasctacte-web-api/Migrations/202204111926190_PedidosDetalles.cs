namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PedidosDetalles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PedidoDetalles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CantidadProducto = c.Int(nullable: false),
                    CantidadFacturada = c.Int(nullable: false),
                    PrecioUnitario = c.Int(nullable: false),
                    IdProducto = c.Int(nullable: false),
                    IdPedido = c.Int(nullable: false),
                    Deleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id); ;
            AddForeignKey("PedidoDetalles", "IdPedido", "Pedidos", "Id");
            AddForeignKey("PedidoDetalles", "IdProducto", "Productos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PedidoDetalles", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.PedidoDetalles", "IdPedido", "dbo.Pedidos");
            DropIndex("dbo.PedidoDetalles", new[] { "IdPedido" });
            DropIndex("dbo.PedidoDetalles", new[] { "IdProducto" });
            DropTable("dbo.PedidoDetalles");
        }
    }
}
