namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newLogTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteCommentLogs",
                c => new
                    {
                        VoteCommentLogID = c.Int(nullable: false, identity: true),
                        tbl_CommentID = c.Int(nullable: false),
                        Vote = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VoteCommentLogID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.tbl_Comment", t => t.tbl_CommentID, cascadeDelete: true)
                .Index(t => t.tbl_CommentID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VisitsLogs",
                c => new
                    {
                        VisitsLogID = c.Int(nullable: false, identity: true),
                        tbl_NewsID = c.Int(nullable: false),
                        Visit = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VisitsLogID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.tbl_News", t => t.tbl_NewsID, cascadeDelete: true)
                .Index(t => t.tbl_NewsID)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitsLogs", "tbl_NewsID", "dbo.tbl_News");
            DropForeignKey("dbo.VisitsLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.VoteCommentLogs", "tbl_CommentID", "dbo.tbl_Comment");
            DropForeignKey("dbo.VoteCommentLogs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.VisitsLogs", new[] { "UserId" });
            DropIndex("dbo.VisitsLogs", new[] { "tbl_NewsID" });
            DropIndex("dbo.VoteCommentLogs", new[] { "UserId" });
            DropIndex("dbo.VoteCommentLogs", new[] { "tbl_CommentID" });
            DropTable("dbo.VisitsLogs");
            DropTable("dbo.VoteCommentLogs");
        }
    }
}
