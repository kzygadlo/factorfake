namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblCommentRefactoring1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.tbl_Comment", name: "UserId", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.tbl_Comment", name: "IX_UserId", newName: "IX_ApplicationUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.tbl_Comment", name: "IX_ApplicationUser_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.tbl_Comment", name: "ApplicationUser_Id", newName: "UserId");
        }
    }
}
