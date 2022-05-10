namespace cuentasctacte_web_api.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Stocks1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Depositos", newName: "Depositos");
            RenameTable(name: "dbo.Productos", newName: "Productos");
            DropForeignKey("dbo.Stocks", "ProductoId", "dbo.Depositos");
            DropForeignKey("dbo.Stocks", "DepositoId", "dbo.Productos");
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "idPersona" });
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "idTipoPersona" });
            DropIndex("dbo.Stocks", new[] { "DepositoId" });
            DropIndex("dbo.Stocks", new[] { "ProductoId" });
            RenameColumn(table: "dbo.Stocks", name: "ProductoId", newName: "IdDeposito");
            RenameColumn(table: "dbo.Stocks", name: "DepositoId", newName: "IdProducto");
            AlterColumn("dbo.Stocks", "IdProducto", c => c.Int());
            AlterColumn("dbo.Stocks", "IdDeposito", c => c.Int());
            CreateIndex("dbo.Personas_Tipos_Personas", "IdPersona");
            CreateIndex("dbo.Personas_Tipos_Personas", "IdTipoPersona");
            CreateIndex("dbo.Stocks", "IdProducto");
            CreateIndex("dbo.Stocks", "IdDeposito");
            AddForeignKey("dbo.Stocks", "IdDeposito", "dbo.Depositos", "Id");
            AddForeignKey("dbo.Stocks", "IdProducto", "dbo.Productos", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "IdProducto", "dbo.Productos");
            DropForeignKey("dbo.Stocks", "IdDeposito", "dbo.Depositos");
            DropIndex("dbo.Stocks", new[] { "IdDeposito" });
            DropIndex("dbo.Stocks", new[] { "IdProducto" });
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "IdTipoPersona" });
            DropIndex("dbo.Personas_Tipos_Personas", new[] { "IdPersona" });
            AlterColumn("dbo.Stocks", "IdDeposito", c => c.Int(nullable: false));
            AlterColumn("dbo.Stocks", "IdProducto", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Stocks", name: "IdProducto", newName: "DepositoId");
            RenameColumn(table: "dbo.Stocks", name: "IdDeposito", newName: "ProductoId");
            CreateIndex("dbo.Stocks", "ProductoId");
            CreateIndex("dbo.Stocks", "DepositoId");
            CreateIndex("dbo.Personas_Tipos_Personas", "idTipoPersona");
            CreateIndex("dbo.Personas_Tipos_Personas", "idPersona");
            AddForeignKey("dbo.Stocks", "DepositoId", "dbo.Productos", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Stocks", "ProductoId", "dbo.Depositos", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Productos", newName: "Productos");
            RenameTable(name: "dbo.Depositos", newName: "Depositos");
        }
    }
}
