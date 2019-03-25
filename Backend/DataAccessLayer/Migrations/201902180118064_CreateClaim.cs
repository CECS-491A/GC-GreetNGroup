namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CreateClaim : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropIndex("dbo.UserClaim", new[] { "ClaimId" });
            DropPrimaryKey("dbo.Claim");
            DropPrimaryKey("dbo.UserClaim");
            AlterColumn("dbo.Claim", "ClaimId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.UserClaim", "ClaimId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Claim", "ClaimId");
            AddPrimaryKey("dbo.UserClaim", new[] { "UId", "ClaimId" });
            CreateIndex("dbo.UserClaim", "ClaimId");
            AddForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim", "ClaimId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropIndex("dbo.UserClaim", new[] { "ClaimId" });
            DropPrimaryKey("dbo.UserClaim");
            DropPrimaryKey("dbo.Claim");
            AlterColumn("dbo.UserClaim", "ClaimId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Claim", "ClaimId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserClaim", new[] { "UId", "ClaimId" });
            AddPrimaryKey("dbo.Claim", "ClaimId");
            CreateIndex("dbo.UserClaim", "ClaimId");
            AddForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim", "ClaimId", cascadeDelete: true);
        }
    }
}
