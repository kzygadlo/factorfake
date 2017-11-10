namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingNewCommentsReplyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_CommentReply",
                c => new
                    {
                        tbl_CommentReplyID = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Visible = c.Boolean(nullable: false),
                        Fakt = c.Int(nullable: false),
                        Fake = c.Int(nullable: false),
                        DateAdd = c.DateTime(nullable: false),
                        ReplyFor = c.Int(nullable: false),
                        tbl_CommentID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.tbl_CommentReplyID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.tbl_Comment", t => t.tbl_CommentID, cascadeDelete: true)
                .Index(t => t.tbl_CommentID)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_CommentReply", "tbl_CommentID", "dbo.tbl_Comment");
            DropForeignKey("dbo.tbl_CommentReply", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.tbl_CommentReply", new[] { "UserId" });
            DropIndex("dbo.tbl_CommentReply", new[] { "tbl_CommentID" });
            DropTable("dbo.tbl_CommentReply");
        }
    }
}
