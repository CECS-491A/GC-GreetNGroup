namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlocks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blocklist",
                c => new
                    {
                        BlockId = c.Int(nullable: false),
                        UserId1 = c.Int(nullable: false),
                        UserId2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlockId)
                .ForeignKey("dbo.User", t => t.UserId1, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId2, cascadeDelete: false)
                .Index(t => t.UserId1)
                .Index(t => t.UserId2);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blocklist", "UserId2", "dbo.User");
            DropForeignKey("dbo.Blocklist", "UserId1", "dbo.User");
            DropIndex("dbo.Blocklist", new[] { "UserId2" });
            DropIndex("dbo.Blocklist", new[] { "UserId1" });
            DropTable("dbo.Blocklist");
        }
    }
}
