namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_Comment", "Comment", c => c.String(nullable: false));
            AlterColumn("dbo.tbl_CommentReply", "Comment", c => c.String(nullable: false));     
        }

        public override void Down()
        {
            AlterColumn("dbo.tbl_CommentReply", "Comment", c => c.String());
            AlterColumn("dbo.tbl_Comment", "Comment", c => c.String());

        }
    }
}
