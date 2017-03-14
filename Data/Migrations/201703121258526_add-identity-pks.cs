namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addidentitypks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.League", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Team", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Result", "FixtureId", "dbo.Fixture");
            DropForeignKey("dbo.Competition", "SeasonId", "dbo.Season");
            DropPrimaryKey("dbo.Competition");
            DropPrimaryKey("dbo.Fixture");
            DropPrimaryKey("dbo.Season");
            AlterColumn("dbo.Competition", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Fixture", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Season", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Competition", "Id");
            AddPrimaryKey("dbo.Fixture", "Id");
            AddPrimaryKey("dbo.Season", "Id");
            AddForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Team", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.League", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Result", "FixtureId", "dbo.Fixture", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Competition", "SeasonId", "dbo.Season", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Competition", "SeasonId", "dbo.Season");
            DropForeignKey("dbo.Result", "FixtureId", "dbo.Fixture");
            DropForeignKey("dbo.League", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Team", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition");
            DropPrimaryKey("dbo.Season");
            DropPrimaryKey("dbo.Fixture");
            DropPrimaryKey("dbo.Competition");
            AlterColumn("dbo.Season", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Fixture", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Competition", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Season", "Id");
            AddPrimaryKey("dbo.Fixture", "Id");
            AddPrimaryKey("dbo.Competition", "Id");
            AddForeignKey("dbo.Competition", "SeasonId", "dbo.Season", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Result", "FixtureId", "dbo.Fixture", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Team", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.League", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
        }
    }
}
