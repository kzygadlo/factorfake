namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class testDodawaniaDynamicznegoSQl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_News", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.tbl_News", "IsActive");

        }

        public override void Down()
        {
            AddColumn("dbo.tbl_News", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.tbl_News", "Active");
        }
    }
}
