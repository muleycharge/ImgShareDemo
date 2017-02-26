namespace ImgShareDemo.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkedInLogInTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.US_LinkedInUser",
                c => new
                    {
                        USL_ID = c.Int(nullable: false, identity: true),
                        USL_USR_ID = c.Int(nullable: false),
                        USL_LinkedInId = c.String(maxLength: 64),
                        USL_Token = c.String(),
                        USL_RefreshToken = c.String(),
                        USL_TokenExpires = c.DateTime(nullable: false),
                        USL_ProfileRequestUrl = c.String(),
                        USL_ProfileImageUrl = c.String(),
                        USL_DateCreated = c.DateTime(nullable: false),
                        USL_DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.USL_ID)
                .ForeignKey("dbo.US_User", t => t.USL_USR_ID, cascadeDelete: true)
                .Index(t => t.USL_USR_ID);
            
            CreateTable(
                "dbo.US_UserSignOn",
                c => new
                    {
                        USO_ID = c.Int(nullable: false, identity: true),
                        USO_State = c.String(maxLength: 32),
                        USO_USR_ID = c.Int(),
                        USO_RedirectUrl = c.String(nullable: false),
                        USO_DateCreated = c.DateTime(nullable: false),
                        USO_DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.USO_ID)
                .ForeignKey("dbo.US_User", t => t.USO_USR_ID)
                .Index(t => t.USO_State, name: "idxUniqueState")
                .Index(t => t.USO_USR_ID);
            
            AddColumn("dbo.US_User", "USR_FirstName", c => c.String());
            AddColumn("dbo.US_User", "USR_LastName", c => c.String());
            AddColumn("dbo.US_User", "USR_Email", c => c.String());
            AddColumn("dbo.US_User", "USR_ImageUrl", c => c.String());
            AddColumn("dbo.US_User", "USR_DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.US_User", "USR_DateModified", c => c.DateTime());
            DropColumn("dbo.US_User", "USR_UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.US_User", "USR_UserName", c => c.String());
            DropForeignKey("dbo.US_UserSignOn", "USO_USR_ID", "dbo.US_User");
            DropForeignKey("dbo.US_LinkedInUser", "USL_USR_ID", "dbo.US_User");
            DropIndex("dbo.US_UserSignOn", new[] { "USO_USR_ID" });
            DropIndex("dbo.US_UserSignOn", "idxUniqueState");
            DropIndex("dbo.US_LinkedInUser", new[] { "USL_USR_ID" });
            DropColumn("dbo.US_User", "USR_DateModified");
            DropColumn("dbo.US_User", "USR_DateCreated");
            DropColumn("dbo.US_User", "USR_ImageUrl");
            DropColumn("dbo.US_User", "USR_Email");
            DropColumn("dbo.US_User", "USR_LastName");
            DropColumn("dbo.US_User", "USR_FirstName");
            DropTable("dbo.US_UserSignOn");
            DropTable("dbo.US_LinkedInUser");
        }
    }
}
