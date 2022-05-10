namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class personas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Personas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Nombre = c.String(),
                    Apellido = c.String(),
                    Telefono = c.String(),
                    DocumentoTipo = c.String(),
                    Documento = c.String(),
                    LineaDeCredito = c.Double(nullable: false),
                    Saldo = c.Double(nullable: false),
                    Deleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Personas");
        }
    }
}
