namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class dropCantidadCuotas : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FacturaDetalles", "CantidadCuotas");
        }

        public override void Down()
        {
            AddColumn("dbo.FacturaDetalles", "CantidadCuotas", c => c.Int(nullable: false));
        }
    }
}
