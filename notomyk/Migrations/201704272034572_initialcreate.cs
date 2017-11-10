namespace notomyk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_Comment",
                c => new
                    {
                        tbl_CommentID = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Visible = c.Boolean(nullable: false),
                        Fakt = c.Int(nullable: false),
                        Fake = c.Int(nullable: false),
                        DateAdd = c.DateTime(nullable: false),
                        ReplyFor = c.Int(nullable: false),
                        tbl_NewsID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.tbl_CommentID)
                .ForeignKey("dbo.tbl_News", t => t.tbl_NewsID, cascadeDelete: true)
                .Index(t => t.tbl_NewsID);

            CreateTable(
                "dbo.tbl_News",
                c => new
                    {
                        tbl_NewsID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Link = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DateAdd = c.DateTime(nullable: false),
                        Fakt = c.Int(nullable: false),
                        Fake = c.Int(nullable: false),
                        tbl_NewspaperID = c.Int(nullable: false),
                        tbl_UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.tbl_NewsID)
                .ForeignKey("dbo.tbl_Newspaper", t => t.tbl_NewspaperID, cascadeDelete: true)
                .ForeignKey("dbo.tbl_User", t => t.tbl_UserID, cascadeDelete: true)
                .Index(t => t.tbl_NewspaperID)
                .Index(t => t.tbl_UserID);

            CreateTable(
                "dbo.tbl_Newspaper",
                c => new
                    {
                        tbl_NewspaperID = c.Int(nullable: false, identity: true),
                        NewspaperName = c.String(),
                        NewspaperLink = c.String(),
                        NewspaperIconLink = c.String(),
                    })
                .PrimaryKey(t => t.tbl_NewspaperID);

            CreateTable(
                "dbo.tbl_User",
                c => new
                    {
                        tbl_UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        UserEmail = c.String(),
                    })
                .PrimaryKey(t => t.tbl_UserID);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.tbl_News", "tbl_UserID", "dbo.tbl_User");
            DropForeignKey("dbo.tbl_News", "tbl_NewspaperID", "dbo.tbl_Newspaper");
            DropForeignKey("dbo.tbl_Comment", "tbl_NewsID", "dbo.tbl_News");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.tbl_News", new[] { "tbl_UserID" });
            DropIndex("dbo.tbl_News", new[] { "tbl_NewspaperID" });
            DropIndex("dbo.tbl_Comment", new[] { "tbl_NewsID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.tbl_User");
            DropTable("dbo.tbl_Newspaper");
            DropTable("dbo.tbl_News");
            DropTable("dbo.tbl_Comment");
        }
    }
}
