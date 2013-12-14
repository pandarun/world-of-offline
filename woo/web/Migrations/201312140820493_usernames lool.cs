namespace web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernameslool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserName", c => c.String());
            AddColumn("dbo.Users", "Provider", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Provider");
            DropColumn("dbo.Users", "UserName");
        }
    }
}
