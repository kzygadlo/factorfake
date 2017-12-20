namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class forumCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumCategories", "IconClass", c => c.String());
            AddColumn("dbo.ForumCategories", "Order", c => c.Int());


            string Sql1 = @"
insert into dbo.ForumCategories
values ('Og�oszenia','info',0),
('Pomys�y', 'idea', 1),
('B��dy','bug',2)
";

            Sql(Sql1);

        }

        public override void Down()
        {
            DropColumn("dbo.ForumCategories", "Order");
            DropColumn("dbo.ForumCategories", "IconClass");
        }
    }
}
