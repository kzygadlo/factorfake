namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredFields1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_News", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_News", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_News", "Description", c => c.String());
            AlterColumn("dbo.tbl_News", "Title", c => c.String());
        }
    }
}
