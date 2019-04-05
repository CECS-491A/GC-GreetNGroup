namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClaims : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.Int(nullable: false),
                        ClaimName = c.String(),
                    })
                .PrimaryKey(t => t.ClaimId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Claim");
        }
    }
}
