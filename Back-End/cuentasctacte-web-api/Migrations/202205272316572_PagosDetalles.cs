namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PagosDetalles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PagosDetalles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Monto = c.Double(nullable: false),
                    IdPago = c.Int(nullable: false),
                    IdCuota = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VencimientoFacturas", t => t.IdCuota, cascadeDelete: true)
                .ForeignKey("dbo.Pagos", t => t.IdPago, cascadeDelete: true)
                .Index(t => t.IdPago)
                .Index(t => t.IdCuota);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PagosDetalles", "IdPago", "dbo.Pagos");
            DropForeignKey("dbo.PagosDetalles", "IdCuota", "dbo.VencimientoFacturas");
            DropIndex("dbo.PagosDetalles", new[] { "IdCuota" });
            DropIndex("dbo.PagosDetalles", new[] { "IdPago" });
            DropTable("dbo.PagosDetalles");
        }
    }
}
