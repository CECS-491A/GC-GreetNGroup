namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUser1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.String(nullable: false, maxLength: 128),
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
