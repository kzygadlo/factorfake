namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addManipulated : DbMigration
    {
        public override void Up()
        {
            string sql1 = @"ALTER TABLE [dbo].[VoteLogs] DROP CONSTRAINT [DF__VoteLogs__Vote__5CD6CB2B]";

            Sql(sql1);


            AlterColumn("dbo.VoteLogs", "Vote", c => c.Int(nullable: false));

            string sql2 = @"ALTER TABLE [dbo].[VoteLogs] ADD  DEFAULT ((0)) FOR [Vote]";

            Sql(sql2);

            string sql3 = @"update VoteLogs set vote = -1 where vote = 0;";

            Sql(sql3);
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VoteLogs", "Vote", c => c.Boolean(nullable: false));
        }
    }
}
