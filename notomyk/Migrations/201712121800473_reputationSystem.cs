namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reputationSystem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Comment", "IsReported", c => c.Boolean(nullable: false));
            AddColumn("dbo.tbl_News", "IsReported", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tags", "TagVotes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "TagVotes", c => c.Int(nullable: false));
            DropColumn("dbo.tbl_News", "IsReported");
            DropColumn("dbo.tbl_Comment", "IsReported");
        }
    }
}
