namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class appSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppSettings", "Type", c => c.String());

            string Sql1 = @"
            update dbo.AppSettings
            set [Type] = 'standard'";
                  

            Sql(Sql1);

        }

        public override void Down()
        {
            DropColumn("dbo.AppSettings", "Type");
        }
    }
}
