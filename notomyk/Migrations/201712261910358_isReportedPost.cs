namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isReportedPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumPosts", "IsReported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumPosts", "IsReported");
        }
    }
}
