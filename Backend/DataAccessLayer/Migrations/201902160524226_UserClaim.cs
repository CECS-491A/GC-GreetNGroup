namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserClaim : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        UId = c.String(nullable: false, maxLength: 128),
                        ClaimId = c.String(nullable: false, maxLength: 128),
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
