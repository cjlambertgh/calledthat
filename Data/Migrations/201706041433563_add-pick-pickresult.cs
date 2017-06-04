namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpickpickresult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PickResult",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PickId = c.Guid(nullable: false),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pick", t => t.PickId, cascadeDelete: true)
                .Index(t => t.PickId);
            
            CreateTable(
                "dbo.Pick",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PlayerId = c.Guid(nullable: false),
                        FixtureId = c.Guid(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        AwayScore = c.Int(nullable: false),
                        Banker = c.Boolean(nullable: false),
                        Double = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fixture", t => t.FixtureId, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.FixtureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PickResult", "PickId", "dbo.Pick");
            DropForeignKey("dbo.Pick", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.Pick", "FixtureId", "dbo.Fixture");
            DropIndex("dbo.Pick", new[] { "FixtureId" });
            DropIndex("dbo.Pick", new[] { "PlayerId" });
            DropIndex("dbo.PickResult", new[] { "PickId" });
            DropTable("dbo.Pick");
            DropTable("dbo.PickResult");
        }
    }
}
