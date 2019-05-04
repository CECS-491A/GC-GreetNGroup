namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJWTTokens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JWTToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JWTToken", "UserId", "dbo.User");
            DropIndex("dbo.JWTToken", new[] { "UserId" });
            DropTable("dbo.JWTToken");
        }
    }
}
