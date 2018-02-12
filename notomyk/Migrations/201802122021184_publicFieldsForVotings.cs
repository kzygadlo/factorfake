namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publicFieldsForVotings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteLogs", "Timestamp", c => c.DateTime());
            AddColumn("dbo.VoteCommentLogs", "Timestamp", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VoteCommentLogs", "Timestamp");
            DropColumn("dbo.VoteLogs", "Timestamp");
        }
    }
}
