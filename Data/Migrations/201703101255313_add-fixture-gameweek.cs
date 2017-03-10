namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfixturegameweek : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameWeek",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompetitionId = c.Guid(nullable: false),
                        Number = c.Int(nullable: false),
                        PickOpenDateTime = c.DateTime(nullable: false),
                        PickCloseDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competition", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.Fixture",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HomeTeamId = c.Guid(nullable: false),
                        AwayTeamId = c.Guid(nullable: false),
                        GameWeekId = c.Guid(nullable: false),
                        KickOffDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Team", t => t.AwayTeamId, cascadeDelete: false)
                .ForeignKey("dbo.GameWeek", t => t.GameWeekId, cascadeDelete: true)
                .ForeignKey("dbo.Team", t => t.HomeTeamId, cascadeDelete: false)
                .Index(t => t.HomeTeamId)
                .Index(t => t.AwayTeamId)
                .Index(t => t.GameWeekId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fixture", "HomeTeamId", "dbo.Team");
            DropForeignKey("dbo.Fixture", "GameWeekId", "dbo.GameWeek");
            DropForeignKey("dbo.Fixture", "AwayTeamId", "dbo.Team");
            DropForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition");
            DropIndex("dbo.Fixture", new[] { "GameWeekId" });
            DropIndex("dbo.Fixture", new[] { "AwayTeamId" });
            DropIndex("dbo.Fixture", new[] { "HomeTeamId" });
            DropIndex("dbo.GameWeek", new[] { "CompetitionId" });
            DropTable("dbo.Fixture");
            DropTable("dbo.GameWeek");
        }
    }
}
