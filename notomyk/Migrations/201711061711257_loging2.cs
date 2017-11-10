namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loging2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VisitsLogs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.VisitsLogs", "tbl_NewsID", "dbo.tbl_News");
            DropIndex("dbo.VisitsLogs", new[] { "tbl_NewsID" });
            DropIndex("dbo.VisitsLogs", new[] { "UserId" });
            AddColumn("dbo.tbl_News", "Visitors", c => c.Int(nullable: false));
            DropTable("dbo.VisitsLogs");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.VisitsLogID);
            
            DropColumn("dbo.tbl_News", "Visitors");
            CreateIndex("dbo.VisitsLogs", "UserId");
            CreateIndex("dbo.VisitsLogs", "tbl_NewsID");
            AddForeignKey("dbo.VisitsLogs", "tbl_NewsID", "dbo.tbl_News", "tbl_NewsID", cascadeDelete: true);
            AddForeignKey("dbo.VisitsLogs", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
