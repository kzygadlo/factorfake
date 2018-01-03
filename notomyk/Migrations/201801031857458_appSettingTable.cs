namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appSettingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            string Sql1 = @"
INSERT INTO [dbo].[AppSettings]
           ([Key]
           ,[Value])
     VALUES
           ( 'CommentsLimitAdmin', '999'),
		   ( 'CommentsLimitModerator', '999'),
		   ( 'CommentsLimitUser', '20'),
		   ( 'CommentsLimitNotConfirmed', '5'),
		   ( 'CommentsAddDelayAdmin', '0'),
		   ( 'CommentsAddDelayModerator', '0'),
		   ( 'CommentsAddDelayUser', '30'),
		   ( 'NewsLimitAdmin', '999'),
		   ( 'NewsLimitModerator', '999'),
		   ( 'NewsLimitUser', '10'),
		   ( 'NewsLimitNotConfirmed', '1'),
		   ( 'NewNewsHours', '48'),
		   ( 'FilterVoting', '4'),
		   ( 'FilterVisitors', '2'),
		   ( 'FilterComments', '2'),
		   ( 'TagStrignLenght', '42'),
		   ( 'MinCommentsForReputation', '2'),
		   ( 'MinNumberVotes', '2')
";
            Sql(Sql1);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppSettings");
        }
    }
}
