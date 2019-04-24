namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditRatings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserRating", "Rating", c => c.Int(nullable: false));
            DropColumn("dbo.UserRating", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRating", "Comment", c => c.String());
            AlterColumn("dbo.UserRating", "Rating", c => c.Single(nullable: false));
        }
    }
}
