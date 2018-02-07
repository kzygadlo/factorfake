namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appSettingsGlobal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppSettingsGlobals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.Boolean(nullable: false),
                        order = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            string sql1 = @"
insert into [dbo].[AppSettingsGlobals]
values 
('SiteEnabled', 1, 1 , 'Strona aktywna'),
('CommentsAdding', 1, 3 , 'Włączone dodawania komentarzy.'),
('NewsAdding', 1, 2 , 'Wlaczone dodawanie newsów.'),
('WaitingRoomActive', 1, 4 , 'Właczona poczekalnia.')
";

            Sql(sql1);

        }
        
        public override void Down()
        {
            DropTable("dbo.AppSettingsGlobals");
        }
    }
}
