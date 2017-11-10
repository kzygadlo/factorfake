namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class singleTableForComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Comment", "parentID", c => c.Int(nullable: false));
            DropColumn("dbo.tbl_Comment", "Replies");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_Comment", "Replies", c => c.Int(nullable: false));
            DropColumn("dbo.tbl_Comment", "parentID");
        }
    }
}
