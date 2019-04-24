namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriends : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        FriendId = c.Int(nullable: false),
                        UserId1 = c.Int(nullable: false),
                        UserId2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.User", t => t.UserId1, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId2, cascadeDelete: false)
                .Index(t => t.UserId1)
                .Index(t => t.UserId2);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "UserId2", "dbo.User");
            DropForeignKey("dbo.Friend", "UserId1", "dbo.User");
            DropIndex("dbo.Friend", new[] { "UserId2" });
            DropIndex("dbo.Friend", new[] { "UserId1" });
            DropTable("dbo.Friend");
        }
    }
}
