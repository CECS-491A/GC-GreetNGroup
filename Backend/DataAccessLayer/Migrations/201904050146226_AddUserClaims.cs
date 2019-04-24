namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserClaims : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        UId = c.Int(nullable: false),
                        ClaimId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UId, t.ClaimId })
                .ForeignKey("dbo.Claim", t => t.ClaimId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UId, cascadeDelete: true)
                .Index(t => t.UId)
                .Index(t => t.ClaimId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserClaim", "UId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropIndex("dbo.UserClaim", new[] { "ClaimId" });
            DropIndex("dbo.UserClaim", new[] { "UId" });
            DropTable("dbo.UserClaim");
        }
    }
}
