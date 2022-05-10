namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Eliminable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedidos", "Deleted", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Pedidos", "Deleted");
        }
    }
}
