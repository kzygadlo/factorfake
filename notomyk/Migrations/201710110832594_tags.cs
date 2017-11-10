namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventTags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NewsID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                        News_tbl_NewsID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbl_News", t => t.News_tbl_NewsID)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.TagID)
                .Index(t => t.News_tbl_NewsID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        TagVotes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventTags", "TagID", "dbo.Tags");
            DropForeignKey("dbo.EventTags", "News_tbl_NewsID", "dbo.tbl_News");
            DropIndex("dbo.EventTags", new[] { "News_tbl_NewsID" });
            DropIndex("dbo.EventTags", new[] { "TagID" });
            DropTable("dbo.Tags");
            DropTable("dbo.EventTags");
        }
    }
}
