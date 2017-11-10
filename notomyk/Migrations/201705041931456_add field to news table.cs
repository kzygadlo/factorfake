namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldtonewstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_News", "ArticleLink", c => c.String());
            AddColumn("dbo.tbl_News", "PictureLink", c => c.String());
            DropColumn("dbo.tbl_News", "Link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_News", "Link", c => c.String());
            DropColumn("dbo.tbl_News", "PictureLink");
            DropColumn("dbo.tbl_News", "ArticleLink");
        }
    }
}
