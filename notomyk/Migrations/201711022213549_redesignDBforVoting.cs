namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redesignDBforVoting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteLogs",
                c => new
                    {
                        VoteLogID = c.Int(nullable: false, identity: true),
                        tbl_NewsID = c.Int(nullable: false),
                        VoteForFakt = c.Int(nullable: false),
                        VoteForFake = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VoteLogID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.tbl_News", t => t.tbl_NewsID, cascadeDelete: true)
                .Index(t => t.tbl_NewsID)
                .Index(t => t.UserId);
            
            DropColumn("dbo.AspNetUsers", "postsNumber");
            DropColumn("dbo.AspNetUsers", "newsAddedNumber");
            DropColumn("dbo.AspNetUsers", "commentsNumber");
            DropColumn("dbo.AspNetUsers", "reputationPoints");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "reputationPoints", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "commentsNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "newsAddedNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "postsNumber", c => c.Int(nullable: false));
            DropForeignKey("dbo.VoteLogs", "tbl_NewsID", "dbo.tbl_News");
            DropForeignKey("dbo.VoteLogs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.VoteLogs", new[] { "UserId" });
            DropIndex("dbo.VoteLogs", new[] { "tbl_NewsID" });
            DropTable("dbo.VoteLogs");
        }
    }
}
