namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_unique_constraints : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GameWeek", new[] { "CompetitionId" });
            DropIndex("dbo.Fixture", new[] { "HomeTeamId" });
            DropIndex("dbo.Fixture", new[] { "AwayTeamId" });
            DropIndex("dbo.Fixture", new[] { "GameWeekId" });
            DropIndex("dbo.Result", new[] { "FixtureId" });
            DropIndex("dbo.PickResult", new[] { "PickId" });
            CreateIndex("dbo.GameWeek", new[] { "CompetitionId", "Number" }, unique: true);
            CreateIndex("dbo.Fixture", new[] { "GameWeekId", "HomeTeamId", "AwayTeamId" }, unique: true);
            CreateIndex("dbo.Result", "FixtureId", unique: true);
            CreateIndex("dbo.PickResult", "PickId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.PickResult", new[] { "PickId" });
            DropIndex("dbo.Result", new[] { "FixtureId" });
            DropIndex("dbo.Fixture", new[] { "GameWeekId", "HomeTeamId", "AwayTeamId" });
            DropIndex("dbo.GameWeek", new[] { "CompetitionId", "Number" });
            CreateIndex("dbo.PickResult", "PickId");
            CreateIndex("dbo.Result", "FixtureId");
            CreateIndex("dbo.Fixture", "GameWeekId");
            CreateIndex("dbo.Fixture", "AwayTeamId");
            CreateIndex("dbo.Fixture", "HomeTeamId");
            CreateIndex("dbo.GameWeek", "CompetitionId");
        }
    }
}
