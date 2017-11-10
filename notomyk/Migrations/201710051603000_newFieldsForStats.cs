namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newFieldsForStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "postsNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "newsAddedNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "commentsNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "reputationPoints", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_News", "Visitors", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_News", "Comments", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_News", "Comments");
            DropColumn("dbo.tbl_News", "Visitors");
            DropColumn("dbo.AspNetUsers", "reputationPoints");
            DropColumn("dbo.AspNetUsers", "commentsNumber");
            DropColumn("dbo.AspNetUsers", "newsAddedNumber");
            DropColumn("dbo.AspNetUsers", "postsNumber");
        }
    }
}
