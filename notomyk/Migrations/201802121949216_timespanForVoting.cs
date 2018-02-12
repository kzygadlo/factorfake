namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timespanForVoting : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "LoginAttempts");
            DropColumn("dbo.AspNetUsers", "LastLoginAttempt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LastLoginAttempt", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LoginAttempts", c => c.Int(nullable: false));
        }
    }
}
