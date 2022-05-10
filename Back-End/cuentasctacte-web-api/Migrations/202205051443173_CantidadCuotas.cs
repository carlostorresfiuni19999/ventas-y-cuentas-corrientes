namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CantidadCuotas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FacturaDetalles", "CantidadCuotas", c => c.Int(nullable: false));
            DropColumn("dbo.FacturaDetalles", "CantidadFacturada");
        }

        public override void Down()
        {
            AddColumn("dbo.FacturaDetalles", "CantidadFacturada", c => c.Int(nullable: false));
            DropColumn("dbo.FacturaDetalles", "CantidadCuotas");
        }
    }
}
