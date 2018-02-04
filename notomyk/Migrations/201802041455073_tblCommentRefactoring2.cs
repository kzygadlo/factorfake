namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblCommentRefactoring2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.tbl_Comment", name: "ApplicationUser_Id", newName: "ApplicationUserAutor_Id");
            RenameIndex(table: "dbo.tbl_Comment", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserAutor_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.tbl_Comment", name: "IX_ApplicationUserAutor_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.tbl_Comment", name: "ApplicationUserAutor_Id", newName: "ApplicationUser_Id");
        }
    }
}
