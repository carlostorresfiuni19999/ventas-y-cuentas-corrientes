namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addCantidadCuotas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "CantidadCuotas", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Facturas", "CantidadCuotas");
        }
    }
}
