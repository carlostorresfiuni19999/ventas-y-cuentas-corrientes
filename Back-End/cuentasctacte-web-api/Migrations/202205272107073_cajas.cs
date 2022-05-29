namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cajas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cajas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NumCaja = c.Int(nullable: false),
                    NombreCaja = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Cajas");
        }
    }
}
