namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonaUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personas", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Personas", "UserName");
        }
    }
}
