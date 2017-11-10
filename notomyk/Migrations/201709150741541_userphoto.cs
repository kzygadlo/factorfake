namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userphoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserPhoto", c => c.Binary());
            DropColumn("dbo.AspNetUsers", "UserIconName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserIconName", c => c.String());
            DropColumn("dbo.AspNetUsers", "UserPhoto");
        }
    }
}
