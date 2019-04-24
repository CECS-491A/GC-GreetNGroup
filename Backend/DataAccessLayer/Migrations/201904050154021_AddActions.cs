namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Action",
                c => new
                    {
                        ActionId = c.Int(nullable: false),
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
