namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newColumnASPuserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserIconName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserIconName");
        }
    }
}
