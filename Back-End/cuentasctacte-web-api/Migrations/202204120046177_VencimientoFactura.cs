namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class VencimientoFactura : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Facturas", t => t.FacturaId, cascadeDelete: true)
                .Index(t => t.FacturaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.VencimientoFacturas", "FacturaId", "dbo.Facturas");
            DropIndex("dbo.VencimientoFacturas", new[] { "FacturaId" });
            DropTable("dbo.VencimientoFacturas");
        }
    }
}
