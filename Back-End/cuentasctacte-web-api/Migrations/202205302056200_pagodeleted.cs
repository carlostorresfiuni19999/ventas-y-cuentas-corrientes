namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pagodeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pagos", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pagos", "Deleted");
        }
    }
}
