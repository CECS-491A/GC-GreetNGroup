namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeIdIdentityKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.Attendee", "EventId", "dbo.Event");
            DropForeignKey("dbo.EventRole", "EventId", "dbo.Event");
            DropForeignKey("dbo.EventTag", "EventId", "dbo.Event");
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropForeignKey("dbo.EventTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.UserTag", "TagId", "dbo.Tag");
            DropPrimaryKey("dbo.Action");
            DropPrimaryKey("dbo.Event");
            DropPrimaryKey("dbo.Blocklist");
            DropPrimaryKey("dbo.Claim");
            DropPrimaryKey("dbo.Tag");
            DropPrimaryKey("dbo.Friend");
            AlterColumn("dbo.Action", "ActionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Event", "EventId", c => c.Int(nullable: false));
            AlterColumn("dbo.Blocklist", "BlockId", c => c.Int(nullable: false));
            AlterColumn("dbo.Claim", "ClaimId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tag", "TagId", c => c.Int(nullable: false));
            AlterColumn("dbo.Friend", "FriendId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Action", "ActionId");
            AddPrimaryKey("dbo.Event", "EventId");
            AddPrimaryKey("dbo.Blocklist", "BlockId");
            AddPrimaryKey("dbo.Claim", "ClaimId");
            AddPrimaryKey("dbo.Tag", "TagId");
            AddPrimaryKey("dbo.Friend", "FriendId");
            AddForeignKey("dbo.UserAction", "ActionId", "dbo.Action", "ActionId", cascadeDelete: true);
            AddForeignKey("dbo.Attendee", "EventId", "dbo.Event", "EventId", cascadeDelete: false);
            AddForeignKey("dbo.EventRole", "EventId", "dbo.Event", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.EventTag", "EventId", "dbo.Event", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim", "ClaimId", cascadeDelete: true);
            AddForeignKey("dbo.EventTag", "TagId", "dbo.Tag", "TagId", cascadeDelete: true);
            AddForeignKey("dbo.UserTag", "TagId", "dbo.Tag", "TagId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.EventTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropForeignKey("dbo.EventTag", "EventId", "dbo.Event");
            DropForeignKey("dbo.EventRole", "EventId", "dbo.Event");
            DropForeignKey("dbo.Attendee", "EventId", "dbo.Event");
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropPrimaryKey("dbo.Friend");
            DropPrimaryKey("dbo.Tag");
            DropPrimaryKey("dbo.Claim");
            DropPrimaryKey("dbo.Blocklist");
            DropPrimaryKey("dbo.Event");
            DropPrimaryKey("dbo.Action");
            AlterColumn("dbo.Friend", "FriendId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Tag", "TagId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Claim", "ClaimId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Blocklist", "BlockId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Event", "EventId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Action", "ActionId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Friend", "FriendId");
            AddPrimaryKey("dbo.Tag", "TagId");
            AddPrimaryKey("dbo.Claim", "ClaimId");
            AddPrimaryKey("dbo.Blocklist", "BlockId");
            AddPrimaryKey("dbo.Event", "EventId");
            AddPrimaryKey("dbo.Action", "ActionId");
            AddForeignKey("dbo.UserTag", "TagId", "dbo.Tag", "TagId", cascadeDelete: true);
            AddForeignKey("dbo.EventTag", "TagId", "dbo.Tag", "TagId", cascadeDelete: true);
            AddForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim", "ClaimId", cascadeDelete: true);
            AddForeignKey("dbo.EventTag", "EventId", "dbo.Event", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.EventRole", "EventId", "dbo.Event", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.Attendee", "EventId", "dbo.Event", "EventId", cascadeDelete: false);
            AddForeignKey("dbo.UserAction", "ActionId", "dbo.Action", "ActionId", cascadeDelete: true);
        }
    }
}
