namespace ImgShareDemo.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialImageLibraryTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.US_UserSignOn", "USO_USR_ID", "dbo.US_User");
            DropIndex("dbo.US_UserSignOn", "idxUniqueState");
            DropIndex("dbo.US_UserSignOn", new[] { "USO_USR_ID" });
            CreateTable(
                "dbo.AS_Asset",
                c => new
                    {
                        ASA_Id = c.Int(nullable: false, identity: true),
                        ASA_USR_Id = c.Int(nullable: false),
                        ASA_Name = c.String(),
                        ASA_Description = c.String(),
                        ASA_SourceUrl = c.String(),
                    })
                .PrimaryKey(t => t.ASA_Id)
                .ForeignKey("dbo.US_User", t => t.ASA_USR_Id, cascadeDelete: true)
                .Index(t => t.ASA_USR_Id);
            
            CreateTable(
                "dbo.AS_AssetTag",
                c => new
                    {
                        AST_Id = c.Int(nullable: false, identity: true),
                        AST_ASA_Id = c.Int(nullable: false),
                        AST_TGT_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AST_Id)
                .ForeignKey("dbo.AS_Asset", t => t.AST_ASA_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.AST_TGT_Id, cascadeDelete: true)
                .Index(t => t.AST_ASA_Id)
                .Index(t => t.AST_TGT_Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TGT_Id = c.Int(nullable: false, identity: true),
                        TGT_TagValue = c.String(),
                        TGT_USR_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TGT_Id)
                .ForeignKey("dbo.US_User", t => t.TGT_USR_Id, cascadeDelete: false)
                .Index(t => t.TGT_USR_Id);
            
            AddColumn("dbo.US_User", "USR_Username", c => c.String(maxLength: 64));
            CreateIndex("dbo.US_User", "USR_Username", unique: true, name: "idxUniqueUsername");
            DropTable("dbo.US_UserSignOn");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.USO_ID);
            
            DropForeignKey("dbo.AS_AssetTag", "AST_TGT_Id", "dbo.Tag");
            DropForeignKey("dbo.Tag", "TGT_USR_Id", "dbo.US_User");
            DropForeignKey("dbo.AS_AssetTag", "AST_ASA_Id", "dbo.AS_Asset");
            DropForeignKey("dbo.AS_Asset", "ASA_USR_Id", "dbo.US_User");
            DropIndex("dbo.Tag", new[] { "TGT_USR_Id" });
            DropIndex("dbo.AS_AssetTag", new[] { "AST_TGT_Id" });
            DropIndex("dbo.AS_AssetTag", new[] { "AST_ASA_Id" });
            DropIndex("dbo.US_User", "idxUniqueUsername");
            DropIndex("dbo.AS_Asset", new[] { "ASA_USR_Id" });
            DropColumn("dbo.US_User", "USR_Username");
            DropTable("dbo.Tag");
            DropTable("dbo.AS_AssetTag");
            DropTable("dbo.AS_Asset");
            CreateIndex("dbo.US_UserSignOn", "USO_USR_ID");
            CreateIndex("dbo.US_UserSignOn", "USO_State", name: "idxUniqueState");
            AddForeignKey("dbo.US_UserSignOn", "USO_USR_ID", "dbo.US_User", "USR_ID");
        }
    }
}
