namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stocks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stocks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Cantidad = c.Int(nullable: false),
                    DepositoId = c.Int(nullable: false),
                    ProductoId = c.Int(nullable: false),
                    Deleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id); ;
            AddForeignKey("dbo.Stocks", "DepositoId", "dbo.Depositos", "Id");
            AddForeignKey("dbo.Stocks", "ProductoId", "dbo.Productos", "Id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "DepositoId", "dbo.Productos");
            DropForeignKey("dbo.Stocks", "ProductoId", "dbo.Depositos");
            DropIndex("dbo.Stocks", new[] { "ProductoId" });
            DropIndex("dbo.Stocks", new[] { "DepositoId" });
            DropTable("dbo.Stocks");
        }
    }
}
