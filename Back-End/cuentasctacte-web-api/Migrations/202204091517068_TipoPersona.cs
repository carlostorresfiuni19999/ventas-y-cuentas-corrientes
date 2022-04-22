namespace cuentasctacte_web_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoPersona : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoPersonas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TipoPersonas");
        }
    }
}
