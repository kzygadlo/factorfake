namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newColumnNewspaperTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Newspaper", "IsActive", c => c.Boolean(nullable: false));

            string sql1 = @"
update [dbo].[tbl_Newspaper]
set IsActive = 1
";

            Sql(sql1);
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Newspaper", "IsActive");
        }
    }
}
