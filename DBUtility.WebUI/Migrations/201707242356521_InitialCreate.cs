namespace DBUtility.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DBActionDetail",
                c => new
                    {
                        ActionID = c.Int(nullable: false),
                        Revision = c.Int(nullable: false),
                        CurrentProgress = c.Int(),
                        ActionResult = c.Int(nullable: false),
                        Detail = c.String(),
                    })
                .PrimaryKey(t => new { t.ActionID, t.Revision })
                .ForeignKey("dbo.DBActionMeta", t => t.ActionID, cascadeDelete: true)
                .Index(t => t.ActionID);
            
            CreateTable(
                "dbo.DBActionMeta",
                c => new
                    {
                        ActionID = c.Int(nullable: false, identity: true),
                        ActionType = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DBActionMeta", "UserID", "dbo.User");
            DropForeignKey("dbo.DBActionDetail", "ActionID", "dbo.DBActionMeta");
            DropIndex("dbo.DBActionMeta", new[] { "UserID" });
            DropIndex("dbo.DBActionDetail", new[] { "ActionID" });
            DropTable("dbo.User");
            DropTable("dbo.DBActionMeta");
            DropTable("dbo.DBActionDetail");
        }
    }
}
