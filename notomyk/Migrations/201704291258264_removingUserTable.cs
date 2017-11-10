namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingUserTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbl_News", "tbl_UserID", "dbo.tbl_User");
            DropIndex("dbo.tbl_News", new[] { "tbl_UserID" });
            DropColumn("dbo.tbl_News", "tbl_UserID");
            DropTable("dbo.tbl_User");

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tbl_User",
                c => new
                    {
                        tbl_UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        UserEmail = c.String(),
                    })
                .PrimaryKey(t => t.tbl_UserID);
            
            AddColumn("dbo.tbl_News", "tbl_UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.tbl_News", "tbl_UserID");
            AddForeignKey("dbo.tbl_News", "tbl_UserID", "dbo.tbl_User", "tbl_UserID", cascadeDelete: true);
        }
    }
}
