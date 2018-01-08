namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlackListAndLoginAttemps : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlackLists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        url = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.url);
            
            AddColumn("dbo.AspNetUsers", "LoginAttempts", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastLoginAttempt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlackLists", new[] { "url" });
            DropColumn("dbo.AspNetUsers", "LastLoginAttempt");
            DropColumn("dbo.AspNetUsers", "LoginAttempts");
            DropTable("dbo.BlackLists");
        }
    }
}
