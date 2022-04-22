namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tipopersonaupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoPersonas", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoPersonas", "Deleted");
        }
    }
}
