namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotasDeCreditos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotasDeCreditos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        FacturaId = c.Int(),
                        NotaDescripcion = c.String(),
                        FechaElaboracion = c.DateTime(nullable: false),
                        Monto = c.Double(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Facturas", t => t.FacturaId)
                .Index(t => t.ClienteId)
                .Index(t => t.FacturaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotasDeCreditos", "FacturaId", "dbo.Facturas");
            DropForeignKey("dbo.NotasDeCreditos", "ClienteId", "dbo.Personas");
            DropIndex("dbo.NotasDeCreditos", new[] { "FacturaId" });
            DropIndex("dbo.NotasDeCreditos", new[] { "ClienteId" });
            DropTable("dbo.NotasDeCreditos");
        }
    }
}
