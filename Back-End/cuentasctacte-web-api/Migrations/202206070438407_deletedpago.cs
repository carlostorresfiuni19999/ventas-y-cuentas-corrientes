namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedpago : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pagos", "Deleted", p => p.Boolean());
        }

        public override void Down()
        {
            DropColumn("dbo.Pagos", "Deleted");
        }
    }
}
