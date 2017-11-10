namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfieldsatribute : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_News", "ArticleLink", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_News", "ArticleLink", c => c.String());
        }
    }
}
