namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PrecioUnitarioDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PedidoDetalles", "PrecioUnitario", c => c.Double(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.PedidoDetalles", "PrecioUnitario", c => c.Int(nullable: false));
        }
    }
}
