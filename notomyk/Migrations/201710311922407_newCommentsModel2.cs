namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newCommentsModel2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbl_CommentReply", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.tbl_CommentReply", "tbl_CommentID", "dbo.tbl_Comment");
            DropIndex("dbo.tbl_CommentReply", new[] { "tbl_CommentID" });
            DropIndex("dbo.tbl_CommentReply", new[] { "UserId" });
            AddColumn("dbo.tbl_Comment", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.tbl_Comment", "Visible");
            DropColumn("dbo.tbl_News", "Comments");
            DropTable("dbo.tbl_CommentReply");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbl_CommentReply",
                c => new
                    {
                        tbl_CommentReplyID = c.Int(nullable: false, identity: true),
                        Comment = c.String(nullable: false),
                        Visible = c.Boolean(nullable: false),
                        Fakt = c.Int(nullable: false),
                        Fake = c.Int(nullable: false),
                        DateAdd = c.DateTime(nullable: false),
                        ReplyFor = c.Int(nullable: false),
                        tbl_CommentID = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.tbl_CommentReplyID);
            
            AddColumn("dbo.tbl_News", "Comments", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_Comment", "Visible", c => c.Boolean(nullable: false));
            DropColumn("dbo.tbl_Comment", "IsActive");
            CreateIndex("dbo.tbl_CommentReply", "UserId");
            CreateIndex("dbo.tbl_CommentReply", "tbl_CommentID");
            AddForeignKey("dbo.tbl_CommentReply", "tbl_CommentID", "dbo.tbl_Comment", "tbl_CommentID", cascadeDelete: true);
            AddForeignKey("dbo.tbl_CommentReply", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
