namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newSchemaForNewsTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_News", "Fakt");
            DropColumn("dbo.tbl_News", "Fake");
            DropColumn("dbo.tbl_News", "Visitors");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_News", "Visitors", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_News", "Fake", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_News", "Fakt", c => c.Int(nullable: false));
        }
    }
}
