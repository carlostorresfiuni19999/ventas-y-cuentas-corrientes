namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Productos : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Depositos", newName: "Depositos");
            RenameTable(name: "dbo.Productos", newName: "Productos");
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
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.IdCliente)
                .ForeignKey("dbo.Personas", t => t.IdVendedor)
                .Index(t => t.IdCliente)
                .Index(t => t.IdVendedor);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Pedidos", "IdVendedor", "dbo.Personas");
            DropForeignKey("dbo.Pedidos", "IdCliente", "dbo.Personas");
            DropIndex("dbo.Pedidos", new[] { "IdVendedor" });
            DropIndex("dbo.Pedidos", new[] { "IdCliente" });
            DropTable("dbo.Pedidos");
            RenameTable(name: "dbo.Productos", newName: "Productos");
            RenameTable(name: "dbo.Depositos", newName: "Depositos");
        }
    }
}
