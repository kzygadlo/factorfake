namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class errorPictureLink : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_News", "PictureLink", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_News", "PictureLink", c => c.String(nullable: false));
        }
    }
}
