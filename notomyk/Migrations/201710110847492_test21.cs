namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class test21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventTags", "News_tbl_NewsID", "dbo.tbl_News");
            DropIndex("dbo.EventTags", new[] { "News_tbl_NewsID" });
            RenameColumn(table: "dbo.EventTags", name: "News_tbl_NewsID", newName: "tbl_NewsID");
            AlterColumn("dbo.EventTags", "tbl_NewsID", c => c.Int(nullable: false));
            CreateIndex("dbo.EventTags", "tbl_NewsID");
            AddForeignKey("dbo.EventTags", "tbl_NewsID", "dbo.tbl_News", "tbl_NewsID", cascadeDelete: true);
            DropColumn("dbo.EventTags", "NewsID");

            string Sql1 = @"
insert into Tags
values ('polityka',0),
('gospodarka',0),
('polska',0),
('europa',0),
('swiat',0),
('usa',0),
('rosja',0),
('uniaeuropejska',0),
('finanse',0)";

            Sql(Sql1);

        }

        public override void Down()
        {
            AddColumn("dbo.EventTags", "NewsID", c => c.Int(nullable: false));
            DropForeignKey("dbo.EventTags", "tbl_NewsID", "dbo.tbl_News");
            DropIndex("dbo.EventTags", new[] { "tbl_NewsID" });
            AlterColumn("dbo.EventTags", "tbl_NewsID", c => c.Int());
            RenameColumn(table: "dbo.EventTags", name: "tbl_NewsID", newName: "News_tbl_NewsID");
            CreateIndex("dbo.EventTags", "News_tbl_NewsID");
            AddForeignKey("dbo.EventTags", "News_tbl_NewsID", "dbo.tbl_News", "tbl_NewsID");
        }
    }
}
