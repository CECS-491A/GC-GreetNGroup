namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class friends : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        FriendId = c.String(nullable: false, maxLength: 128),
                        UserId1 = c.String(nullable: false, maxLength: 128),
                        UserId2 = c.String(nullable: false, maxLength: 128),
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
