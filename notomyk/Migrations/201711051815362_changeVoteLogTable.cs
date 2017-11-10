namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeVoteLogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VoteLogs", "Vote", c => c.Boolean(nullable: false));
            DropColumn("dbo.VoteLogs", "VoteForFakt");
            DropColumn("dbo.VoteLogs", "VoteForFake");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VoteLogs", "VoteForFake", c => c.Int(nullable: false));
            AddColumn("dbo.VoteLogs", "VoteForFakt", c => c.Int(nullable: false));
            DropColumn("dbo.VoteLogs", "Vote");
        }
    }
}
