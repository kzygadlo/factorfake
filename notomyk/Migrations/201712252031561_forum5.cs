namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forum5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Fakt = c.Int(nullable: false),
                        Fake = c.Int(nullable: false),
                        DateAdd = c.DateTime(nullable: false),
                        DateModify = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Parent_ID = c.Int(),
                        Topic_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ForumPosts", t => t.Parent_ID)
                .ForeignKey("dbo.ForumTopics", t => t.Topic_ID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Parent_ID)
                .Index(t => t.Topic_ID);
            
            AlterColumn("dbo.tbl_News", "PictureLink", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumPosts", "Topic_ID", "dbo.ForumTopics");
            DropForeignKey("dbo.ForumPosts", "Parent_ID", "dbo.ForumPosts");
            DropForeignKey("dbo.ForumPosts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ForumPosts", new[] { "Topic_ID" });
            DropIndex("dbo.ForumPosts", new[] { "Parent_ID" });
            DropIndex("dbo.ForumPosts", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.tbl_News", "PictureLink", c => c.String());
            DropTable("dbo.ForumPosts");
        }
    }
}
