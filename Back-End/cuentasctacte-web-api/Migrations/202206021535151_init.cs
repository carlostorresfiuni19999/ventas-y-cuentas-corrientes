namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
    "dbo.Course",
    c => new
    {
        CourseID = c.Int(nullable: false),
        Title = c.String(),
        Credits = c.Int(nullable: false),
    })
    .PrimaryKey(t => t.CourseID);

            CreateTable(
                "dbo.Pagos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Monto = c.Double(nullable: false),
                    IdCliente = c.Int(nullable: false),
                    IdCaja = c.Int(nullable: false),
                    IdCajero = c.Int(),
                    FachaPago = c.DateTime(),
                    IdVencimientoFactura = c.Int(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VencimientoFacturas", t => t.IdVencimientoFactura, cascadeDelete: false)
                .ForeignKey("dbo.Cajas", t => t.IdCaja, cascadeDelete: false)
                .ForeignKey("dbo.Personas", t => t.IdCajero, cascadeDelete: false)
                .ForeignKey("dbo.Personas", t => t.IdCliente, cascadeDelete: false)
                .Index(t => t.IdVencimientoFactura)
                .Index(t => t.IdCaja)
                .Index(t => t.IdCajero)
                .Index(t => t.IdCliente);

            CreateTable(
                "dbo.FormasPagos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MetodoPago = c.String(),
                    IdPago = c.Int(nullable: false),
                    Monto = c.Double(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pagos", t => t.IdPago)
                .Index(t => t.IdPago);
        }

        public override void Down()
        {
            DropForeignKey("dbo.FormasPagos", "IdPago", "dbo.Pagos");
            DropForeignKey("dbo.Pagos", "IdVencimientoFacturas", "dbo.VencimientoFacturas");
            DropForeignKey("dbo.Pagos", "IdCliente", "dbo.Personas");
            DropForeignKey("dbo.Pagos", "IdCajero", "dbo.Personas");
            DropForeignKey("dbo.Pagos", "IdCaja", "dbo.Cajas");
            DropIndex("dbo.FormasPagos", new[] { "IdPago" });
            DropIndex("dbo.Pagos", new[] { "IdCliente" });
            DropIndex("dbo.Pagos", new[] { "IdCajero" });
            DropIndex("dbo.Pagos", new[] { "IdCaja" });
            DropIndex("dbo.Pagos", new[] { "IdVencimientoFactura" });
            DropTable("dbo.FormasPagos");
            DropTable("dbo.Pagos");
        }
    }
}
