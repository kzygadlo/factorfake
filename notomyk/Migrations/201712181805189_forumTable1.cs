namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forumTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.ID);


            string Sql1 = @"
insert into dbo.ForumCategories
values ('Ogloszenia'),
('Bledy'),
('Pomysly')";

            Sql(Sql1);
            
            CreateTable(
                "dbo.ForumTopics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Description = c.String(),
                        DateAdd = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Visitors = c.Int(nullable: false),
                        IsReported = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ForumCategory_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.ForumCategories", t => t.ForumCategory_ID)
                .Index(t => t.UserId)
                .Index(t => t.ForumCategory_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumTopics", "ForumCategory_ID", "dbo.ForumCategories");
            DropForeignKey("dbo.ForumTopics", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ForumTopics", new[] { "ForumCategory_ID" });
            DropIndex("dbo.ForumTopics", new[] { "UserId" });
            DropTable("dbo.ForumTopics");
            DropTable("dbo.ForumCategories");
        }
    }
}
