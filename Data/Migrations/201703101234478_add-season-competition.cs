namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addseasoncompetition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Competition",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        SeasonId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Season", t => t.SeasonId, cascadeDelete: true)
                .Index(t => t.SeasonId);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CurrentSeasonYear = c.Int(nullable: false),
                        SeasonName = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Team", "CompetitionId", c => c.Guid(nullable: false));
            AddColumn("dbo.Team", "SeasonId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Team", "CompetitionId");
            CreateIndex("dbo.Team", "SeasonId");
            AddForeignKey("dbo.Team", "SeasonId", "dbo.Season", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Team", "CompetitionId", "dbo.Competition", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team", "CompetitionId", "dbo.Competition");
            DropForeignKey("dbo.Team", "SeasonId", "dbo.Season");
            DropForeignKey("dbo.Competition", "SeasonId", "dbo.Season");
            DropIndex("dbo.Competition", new[] { "SeasonId" });
            DropIndex("dbo.Team", new[] { "SeasonId" });
            DropIndex("dbo.Team", new[] { "CompetitionId" });
            DropColumn("dbo.Team", "SeasonId");
            DropColumn("dbo.Team", "CompetitionId");
            DropTable("dbo.Season");
            DropTable("dbo.Competition");
        }
    }
}
