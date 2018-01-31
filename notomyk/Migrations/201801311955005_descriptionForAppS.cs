namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descriptionForAppS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppSettings", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppSettings", "Description");
        }
    }
}
