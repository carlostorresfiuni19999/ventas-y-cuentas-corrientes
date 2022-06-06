namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class saldocaja : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cajas", "Saldo", c => c.Double(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Cajas", "Saldo");
        }
    }
}
