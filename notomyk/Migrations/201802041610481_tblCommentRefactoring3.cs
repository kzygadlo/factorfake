namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblCommentRefactoring3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Comment", "NewContent", c => c.String());
            AddColumn("dbo.tbl_Comment", "EditorName", c => c.String());
            AddColumn("dbo.tbl_Comment", "EditDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Comment", "EditDate");
            DropColumn("dbo.tbl_Comment", "EditorName");
            DropColumn("dbo.tbl_Comment", "NewContent");
        }
    }
}
