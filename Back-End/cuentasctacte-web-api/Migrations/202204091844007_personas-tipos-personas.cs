namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class personastipospersonas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Personas_Tipos_Personas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    idPersona = c.Int(),
                    idTipoPersona = c.Int(),
                    Deleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personas", t => t.idPersona)
                .ForeignKey("dbo.TipoPersonas", t => t.idTipoPersona)
                .Index(t => t.idPersona)
                .Index(t => t.idTipoPersona);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Personas_Tipos_Personas", "idTipoPersona", "dbo.TipoPersonas");
            DropForeignKey("dbo.Personas_Tipos_Personas", "idPersona", "dbo.Personas");
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "idTipoPersona" });
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "idPersona" });
            DropTable("dbo.Personas_Tipos_Personas");
        }
    }
}
