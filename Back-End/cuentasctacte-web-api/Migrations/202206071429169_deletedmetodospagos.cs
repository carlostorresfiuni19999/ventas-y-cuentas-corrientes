namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedmetodospagos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormasPagos", "Deleted", c => c.Boolean());
            
        }
        
        public override void Down()
        {
           
            DropColumn("dbo.FormasPagos", "Deleted");
        }
    }
}
