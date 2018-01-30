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

            string Sql2 = @"
            INSERT INTO [dbo].[AppSettings]
           ([Key]
           ,[Value]
            ,[Type])
            VALUES
            ( 'AddingComments', '1', 'global'),
		    ( 'AddingNews', '1', 'global'),
            ( 'ShowComments', '1', 'global'),
            ( 'ShowUsersDetail', '1', 'global'),
            ( 'WaitingRoomActive', '1', 'global')";                       

            Sql(Sql1);
            Sql(Sql2);
        }

        public override void Down()
        {
            DropColumn("dbo.AppSettings", "Type");
        }
    }
}
