namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRepliesCounterColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Comment", "Replies", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Comment", "Replies");
        }
    }
}
