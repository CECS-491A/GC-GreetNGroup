namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTag",
                c => new
                    {
                        TagId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagId, t.UserId })
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TagId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTag", "UserId", "dbo.User");
            DropForeignKey("dbo.UserTag", "TagId", "dbo.Tag");
            DropIndex("dbo.UserTag", new[] { "UserId" });
            DropIndex("dbo.UserTag", new[] { "TagId" });
            DropTable("dbo.UserTag");
        }
    }
}
