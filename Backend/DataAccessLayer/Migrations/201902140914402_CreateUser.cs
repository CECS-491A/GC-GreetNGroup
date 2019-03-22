namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        DoB = c.DateTime(nullable: false),
                        SecurityQuestion = c.String(nullable: false),
                        SecurityAnswer = c.String(nullable: false),
                        IsActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
