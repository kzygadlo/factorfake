namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddescriptionfiled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_News", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_News", "Description");
        }
    }
}
