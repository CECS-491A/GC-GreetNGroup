namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnjwttoken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JWTToken", "isValid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JWTToken", "isValid");
        }
    }
}
