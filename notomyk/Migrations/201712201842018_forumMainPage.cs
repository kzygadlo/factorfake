namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forumMainPage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumTopics", "OnMainPage", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForumTopics", "OnMainPage");
        }
    }
}
