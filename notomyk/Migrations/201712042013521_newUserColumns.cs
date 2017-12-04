namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newUserColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NewsCounter", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastNewsAdded", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "CommentsCounter", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastCommentAdded", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastCommentAdded");
            DropColumn("dbo.AspNetUsers", "CommentsCounter");
            DropColumn("dbo.AspNetUsers", "LastNewsAdded");
            DropColumn("dbo.AspNetUsers", "NewsCounter");
        }
    }
}
