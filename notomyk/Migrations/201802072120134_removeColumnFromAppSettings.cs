namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeColumnFromAppSettings : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AppSettings", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppSettings", "Type", c => c.String());
        }
    }
}
