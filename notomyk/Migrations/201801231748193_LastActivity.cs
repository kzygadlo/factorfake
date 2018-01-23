namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AccountCreateDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LastActivity", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastActivity");
            DropColumn("dbo.AspNetUsers", "AccountCreateDate");
        }
    }
}
