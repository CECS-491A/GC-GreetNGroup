namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAction",
                c => new
                    {
                        ActionTime = c.DateTime(nullable: false),
                        SessionId = c.String(nullable: false, maxLength: 128),
                        Action = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ActionTime, t.SessionId, t.Action })
                .ForeignKey("dbo.Action", t => t.Action, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.Action)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAction", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAction", "Action", "dbo.Action");
            DropIndex("dbo.UserAction", new[] { "UserId" });
            DropIndex("dbo.UserAction", new[] { "Action" });
            DropTable("dbo.UserAction");
        }
    }
}
