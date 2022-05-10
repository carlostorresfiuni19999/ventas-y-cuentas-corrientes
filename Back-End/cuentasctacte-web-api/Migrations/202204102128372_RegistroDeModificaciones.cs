namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RegistroDeModificaciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Modificaciones",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModificadoPor = c.String(),
                    IdPersona = c.Int(nullable: false),
                    Data = c.String(),
                    ModificacionTipo = c.String(),
                    FechaModificacion = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Modificaciones");
        }
    }
}
