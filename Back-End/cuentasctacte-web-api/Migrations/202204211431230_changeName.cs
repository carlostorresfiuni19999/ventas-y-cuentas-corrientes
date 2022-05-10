namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class changeName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Productos", newName: "Productos");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.Productos", newName: "Productos");
        }
    }
}
