namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class metodospagos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MetodosPagos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Monto = c.Double(nullable: false),
                    IdPago = c.Int(nullable: false),
                    IdFormasPagos = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormasPagos", t => t.IdFormasPagos, cascadeDelete: false)
                .ForeignKey("dbo.Pagos", t => t.IdPago, cascadeDelete: false)
                .Index(t => t.IdPago)
                .Index(t => t.IdFormasPagos);

        }

        public override void Down()
        {
            DropForeignKey("dbo.MetodosPagos", "IdPago", "dbo.Pagos");
            DropForeignKey("dbo.MetodosPagos", "IdFormasPagos", "dbo.FormasPagos");
            DropIndex("dbo.MetodosPagos", new[] { "IdFormasPagos" });
            DropIndex("dbo.MetodosPagos", new[] { "IdPago" });
            DropTable("dbo.MetodosPagos");
        }
    }
}
