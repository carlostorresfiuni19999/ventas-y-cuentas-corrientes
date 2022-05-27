namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Pagos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pagos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MontoTotal = c.Double(nullable: false),
                    FechaPago = c.DateTime(nullable: false),
                    IdCliente = c.Int(nullable: false),
                    IdCaja = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cajas", t => t.IdCaja, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.IdCliente, cascadeDelete: true)
                .Index(t => t.IdCliente)
                .Index(t => t.IdCaja);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Pagos", "IdCliente", "dbo.Personas");
            DropForeignKey("dbo.Pagos", "IdCaja", "dbo.Cajas");
            DropIndex("dbo.Pagos", new[] { "IdCaja" });
            DropIndex("dbo.Pagos", new[] { "IdCliente" });
            DropTable("dbo.Pagos");
        }
    }
}
