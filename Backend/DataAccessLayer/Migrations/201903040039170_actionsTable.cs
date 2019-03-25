namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class actionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Action",
                c => new
                    {
                        ActionId = c.String(nullable: false, maxLength: 128),
                        ActionName = c.String(),
                    })
                .PrimaryKey(t => t.ActionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Action");
        }
    }
}
