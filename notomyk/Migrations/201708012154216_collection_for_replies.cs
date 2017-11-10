namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collection_for_replies : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tbl_Comment", "ReplyFor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbl_Comment", "ReplyFor", c => c.Int(nullable: false));
        }
    }
}
