namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_eventcheckincode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "EventCheckinCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "EventCheckinCode");
        }
    }
}
