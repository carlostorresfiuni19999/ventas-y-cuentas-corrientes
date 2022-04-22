namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeNameDeposito : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Depositos", newName: "Depositos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Depositos", newName: "Depositos");
        }
    }
}
