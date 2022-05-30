namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class modificacionpago : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pagos", "IdCajero", c => c.Int(nullable: false));
            CreateIndex("dbo.Pagos", "IdCajero");
            AddForeignKey("dbo.Pagos", "IdCajero", "dbo.Personas", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Pagos", "IdCajero", "dbo.Personas");
            DropIndex("dbo.Pagos", new[] { "IdCajero" });
            DropColumn("dbo.Pagos", "IdCajero");
        }
    }
}
