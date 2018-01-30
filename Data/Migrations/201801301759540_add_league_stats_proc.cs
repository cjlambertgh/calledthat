namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_league_stats_proc : DbMigration
    {
        public override void Up()
        {
            Sql(CreateProcSql);
        }
        
        public override void Down()
        {
            Sql(DropProcSql);
        }

        private static string DropProcSql = "DROP PROCEDURE [dbo].[uspGetLeagueStats]";

        private static string CreateProcSql = @"CREATE procedure [dbo].[uspGetLeagueStats] 
@leagueId varchar(255) 
as
DECLARE @stats TABLE (
  Playerid uniqueidentifier,
  GameweekNumber int,
  Points int,
  Stat nvarchar(64)
)

DECLARE @data TABLE (
  playerId uniqueidentifier,
  GameweekNumber int,
  GameweekPoints int
)

INSERT INTO @data
  SELECT
    p.Id playerId,
    g.Number AS GameweekNumber,
    SUM(pr.Points) GameweekPoints
  FROM player p
  JOIN PlayerLeagues pl
    ON pl.PlayerId = p.Id
  JOIN League l
    ON pl.LeagueId = l.Id
  LEFT JOIN Pick pic
    ON pic.PlayerId = p.Id
  LEFT JOIN Fixture f
    ON pic.FixtureId = f.Id
  LEFT JOIN GameWeek g
    ON f.GameWeekId = g.Id
  LEFT JOIN PickResult pr
    ON pic.id = pr.PickId
  LEFT JOIN Competition c
    ON l.CompetitionId = c.Id
  WHERE l.id = @leagueId
  GROUP BY p.id,
           g.Number


INSERT INTO @stats (PlayerId, GameweekNumber, Points, Stat)
  SELECT
    playerId,
    GameWeekNumber,
    GameweekPoints,
    'HighestScore'
  FROM @data d
  JOIN (SELECT
    MAX(GameweekPoints) points
  FROM @data) AS highest
    ON highest.points = d.GameweekPoints

INSERT INTO @stats (PlayerId, GameweekNumber, Points, Stat)
  SELECT
    playerId,
    GameWeekNumber,
    GameweekPoints,
    'LowestScore'
  FROM @data d
  JOIN (SELECT
    MIN(GameweekPoints) points
  FROM @data) AS highest
    ON highest.points = d.GameweekPoints

SELECT
  *
FROM @Stats
GO

";
    }
}
