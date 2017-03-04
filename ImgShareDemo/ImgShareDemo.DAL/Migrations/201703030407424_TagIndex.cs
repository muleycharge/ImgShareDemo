namespace ImgShareDemo.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tag", new[] { "TGT_USR_Id" });
            AlterColumn("dbo.Tag", "TGT_TagValue", c => c.String(maxLength: 64));
            CreateIndex("dbo.Tag", new[] { "TGT_USR_Id", "TGT_TagValue" }, unique: true, name: "idxUniqueUserTag");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tag", "idxUniqueUserTag");
            AlterColumn("dbo.Tag", "TGT_TagValue", c => c.String());
            CreateIndex("dbo.Tag", "TGT_USR_Id");
        }
    }
}
