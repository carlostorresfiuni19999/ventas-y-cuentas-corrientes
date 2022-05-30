namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddCajero : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cajas", "IdCajero", c => c.Int(nullable: false));
            CreateIndex("dbo.Cajas", "IdCajero");
            AddForeignKey("dbo.Cajas", "IdCajero", "dbo.Personas", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Cajas", "IdCajero", "dbo.Personas");
            DropIndex("dbo.Cajas", new[] { "IdCajero" });
            DropColumn("dbo.Cajas", "IdCajero");
        }
    }
}
