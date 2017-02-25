namespace ImgShareDemo.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.US_User",
                c => new
                    {
                        USR_ID = c.Int(nullable: false, identity: true),
                        USR_UserName = c.String(),
                    })
                .PrimaryKey(t => t.USR_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.US_User");
        }
    }
}
