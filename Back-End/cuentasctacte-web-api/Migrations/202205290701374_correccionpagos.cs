namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correccionpagos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MetodosPagos", "IdFormasPagos", "dbo.FormasPagos");
            DropIndex("dbo.MetodosPagos", new[] { "IdFormasPagos" });
            RenameColumn(table: "dbo.FormasPagos", name: "IdPago", newName: "IdPagoDetalle");
            RenameIndex(table: "dbo.FormasPagos", name: "IX_IdPago", newName: "IX_IdPagoDetalle");
            AddColumn("dbo.FormasPagos", "IdMetodosPagos", c => c.Int(nullable: false));
            AddColumn("dbo.MetodosPagos", "FechaDelPago", c => c.DateTime(nullable: false));
            CreateIndex("dbo.FormasPagos", "IdMetodosPagos");
            AddForeignKey("dbo.FormasPagos", "IdMetodosPagos", "dbo.MetodosPagos", "Id", cascadeDelete: true);
            DropColumn("dbo.MetodosPagos", "IdFormasPagos");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MetodosPagos", "IdFormasPagos", c => c.Int(nullable: false));
            DropForeignKey("dbo.FormasPagos", "IdMetodosPagos", "dbo.MetodosPagos");
            DropIndex("dbo.FormasPagos", new[] { "IdMetodosPagos" });
            DropColumn("dbo.MetodosPagos", "FechaDelPago");
            DropColumn("dbo.FormasPagos", "IdMetodosPagos");
            RenameIndex(table: "dbo.FormasPagos", name: "IX_IdPagoDetalle", newName: "IX_IdPago");
            RenameColumn(table: "dbo.FormasPagos", name: "IdPagoDetalle", newName: "IdPago");
            CreateIndex("dbo.MetodosPagos", "IdFormasPagos");
            AddForeignKey("dbo.MetodosPagos", "IdFormasPagos", "dbo.FormasPagos", "Id", cascadeDelete: true);
        }
    }
}
