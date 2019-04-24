namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTag",
                c => new
                    {
                        TagId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagId, t.UserId });
            
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        FriendId = c.Int(nullable: false),
                        UserId1 = c.Int(nullable: false),
                        UserId2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId);
            
            CreateTable(
                "dbo.EventRole",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        RoleName = c.String(nullable: false, maxLength: 128),
                        MaxRoleCount = c.Int(nullable: false),
                        RequiredRole = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.RoleName });
            
            CreateTable(
                "dbo.Blocklist",
                c => new
                    {
                        BlockId = c.Int(nullable: false),
                        UserId1 = c.Int(nullable: false),
                        UserId2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlockId);
            
            CreateIndex("dbo.UserTag", "UserId");
            CreateIndex("dbo.UserTag", "TagId");
            CreateIndex("dbo.Friend", "UserId2");
            CreateIndex("dbo.Friend", "UserId1");
            CreateIndex("dbo.EventRole", "EventId");
            CreateIndex("dbo.Blocklist", "UserId2");
            CreateIndex("dbo.Blocklist", "UserId1");
            AddForeignKey("dbo.UserTag", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserTag", "TagId", "dbo.Tag", "TagId", cascadeDelete: true);
            AddForeignKey("dbo.Friend", "UserId2", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Friend", "UserId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.EventRole", "EventId", "dbo.Event", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.Blocklist", "UserId2", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Blocklist", "UserId1", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
