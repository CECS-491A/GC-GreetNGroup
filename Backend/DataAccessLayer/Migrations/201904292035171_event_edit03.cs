namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class event_edit03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "IsEventExpired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "IsEventExpired");
        }
    }
}
