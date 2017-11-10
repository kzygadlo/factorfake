namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentTableRed2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Comment", "Parenttbl_CommentID", c => c.Int());
            CreateIndex("dbo.tbl_Comment", "Parenttbl_CommentID");
            AddForeignKey("dbo.tbl_Comment", "Parenttbl_CommentID", "dbo.tbl_Comment", "tbl_CommentID");
            DropColumn("dbo.tbl_Comment", "parentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_Comment", "parentID", c => c.Int(nullable: false));
            DropForeignKey("dbo.tbl_Comment", "Parenttbl_CommentID", "dbo.tbl_Comment");
            DropIndex("dbo.tbl_Comment", new[] { "Parenttbl_CommentID" });
            DropColumn("dbo.tbl_Comment", "Parenttbl_CommentID");
        }
    }
}
