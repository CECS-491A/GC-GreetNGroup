namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRating",
                c => new
                    {
                        RaterId1 = c.Int(nullable: false),
                        RatedId1 = c.Int(nullable: false),
                        Rating = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => new { t.RaterId1, t.RatedId1 })
                .ForeignKey("dbo.User", t => t.RatedId1, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.RaterId1, cascadeDelete: false)
                .Index(t => t.RaterId1)
                .Index(t => t.RatedId1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRating", "RaterId1", "dbo.User");
            DropForeignKey("dbo.UserRating", "RatedId1", "dbo.User");
            DropIndex("dbo.UserRating", new[] { "RatedId1" });
            DropIndex("dbo.UserRating", new[] { "RaterId1" });
            DropTable("dbo.UserRating");
        }
    }
}
