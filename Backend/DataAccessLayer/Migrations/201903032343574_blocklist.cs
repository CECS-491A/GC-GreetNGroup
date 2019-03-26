namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class blocklist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blocklist",
                c => new
                    {
                        BlockId = c.String(nullable: false, maxLength: 128),
                        UserId1 = c.String(nullable: false, maxLength: 128),
                        UserId2 = c.String(nullable: false, maxLength: 128),
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
