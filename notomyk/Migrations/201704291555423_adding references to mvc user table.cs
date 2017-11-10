namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingreferencestomvcusertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Comment", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.tbl_News", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.tbl_Comment", "UserId");
            CreateIndex("dbo.tbl_News", "UserId");
            AddForeignKey("dbo.tbl_News", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.tbl_Comment", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_Comment", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.tbl_News", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.tbl_News", new[] { "UserId" });
            DropIndex("dbo.tbl_Comment", new[] { "UserId" });
            DropColumn("dbo.tbl_News", "UserId");
            DropColumn("dbo.tbl_Comment", "UserId");
        }
    }
}
