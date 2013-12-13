namespace web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Since = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Since = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.EventUsers",
                c => new
                    {
                        Event_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.User_Id })
                .ForeignKey("dbo.Events", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.EventUsers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Messages", "EventId", "dbo.Events");
            DropForeignKey("dbo.Activities", "UserId", "dbo.Users");
            DropIndex("dbo.EventUsers", new[] { "User_Id" });
            DropIndex("dbo.EventUsers", new[] { "Event_Id" });
            DropIndex("dbo.Messages", new[] { "EventId" });
            DropIndex("dbo.Activities", new[] { "UserId" });
            DropTable("dbo.EventUsers");
            DropTable("dbo.Messages");
            DropTable("dbo.Events");
            DropTable("dbo.Users");
            DropTable("dbo.Activities");
        }
    }
}
