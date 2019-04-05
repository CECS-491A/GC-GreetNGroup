namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventTag",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.TagId })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.EventTag", "EventId", "dbo.Event");
            DropIndex("dbo.EventTag", new[] { "TagId" });
            DropIndex("dbo.EventTag", new[] { "EventId" });
            DropTable("dbo.EventTag");
        }
    }
}
