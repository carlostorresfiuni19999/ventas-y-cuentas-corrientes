namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class extendsdata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cajas", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.FormasPagos", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.MetodosPagos", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PagosDetalles", "Deleted", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.PagosDetalles", "Deleted");
            DropColumn("dbo.MetodosPagos", "Deleted");
            DropColumn("dbo.FormasPagos", "Deleted");
            DropColumn("dbo.Cajas", "Deleted");
        }
    }
}
