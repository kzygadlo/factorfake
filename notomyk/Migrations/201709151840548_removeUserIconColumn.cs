namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserIconColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "UserIconName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserIconName", c => c.String());
        }
    }
}
