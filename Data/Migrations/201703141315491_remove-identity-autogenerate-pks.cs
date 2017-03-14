namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeidentityautogeneratepks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Team", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.League", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Competition", "SeasonId", "dbo.Season");
            DropPrimaryKey("dbo.Competition");
            DropPrimaryKey("dbo.Season");
            AlterColumn("dbo.Competition", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Season", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Competition", "Id");
            AddPrimaryKey("dbo.Season", "Id");
            AddForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Team", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.League", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Competition", "SeasonId", "dbo.Season", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Competition", "SeasonId", "dbo.Season");
            DropForeignKey("dbo.League", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Team", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition");
            DropPrimaryKey("dbo.Season");
            DropPrimaryKey("dbo.Competition");
            AlterColumn("dbo.Season", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Competition", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Season", "Id");
            AddPrimaryKey("dbo.Competition", "Id");
            AddForeignKey("dbo.Competition", "SeasonId", "dbo.Season", "Id", cascadeDelete: true);
            AddForeignKey("dbo.League", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Team", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GameWeek", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: true);
        }
    }
}
