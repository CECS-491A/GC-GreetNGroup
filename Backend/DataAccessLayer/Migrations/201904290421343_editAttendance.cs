namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editAttendance : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Attendee", "RoleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attendee", "RoleName", c => c.String());
        }
    }
}
