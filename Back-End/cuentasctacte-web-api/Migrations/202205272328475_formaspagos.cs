namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class formaspagos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormasPagos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FormaDePago = c.String(),
                    Monto = c.Double(nullable: false),
                    IdPago = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pagos", t => t.IdPago, cascadeDelete: true)
                .Index(t => t.IdPago);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FormasPagos", "IdPago", "dbo.Pagos");
            DropIndex("dbo.FormasPagos", new[] { "IdPago" });
            DropTable("dbo.FormasPagos");
        }
    }
}
