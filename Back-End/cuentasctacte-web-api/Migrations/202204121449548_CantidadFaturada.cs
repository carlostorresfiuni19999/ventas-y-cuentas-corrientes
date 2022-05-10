namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CantidadFaturada : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacturaDetalles", "CantidadFacturada", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FacturaDetalles", "CantidadFacturada");
        }
    }
}
