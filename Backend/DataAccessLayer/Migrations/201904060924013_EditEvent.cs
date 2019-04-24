namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "EventLocation", c => c.String(nullable: false));
            AddColumn("dbo.User", "EventCreationCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "EventCreationCount");
            DropColumn("dbo.Event", "EventLocation");
        }
    }
}
