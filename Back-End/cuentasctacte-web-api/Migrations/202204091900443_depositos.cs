namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class depositos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Depositos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreDeposito = c.String(),
                        DescripcionDeposito = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Depositos");
        }
    }
}
