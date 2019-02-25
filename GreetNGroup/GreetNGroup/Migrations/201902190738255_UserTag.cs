namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTag",
                c => new
                    {
                        TagId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
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
