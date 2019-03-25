namespace DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class eventTagAndTag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventTag",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        TagId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.EventId, t.TagId })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagId = c.String(nullable: false, maxLength: 128),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.EventTag", "EventId", "dbo.Event");
            DropIndex("dbo.EventTag", new[] { "TagId" });
            DropIndex("dbo.EventTag", new[] { "EventId" });
            DropTable("dbo.Tag");
            DropTable("dbo.EventTag");
        }
    }
}
