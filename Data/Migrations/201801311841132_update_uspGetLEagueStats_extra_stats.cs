namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_uspGetLEagueStats_extra_stats : DbMigration
    {
        public override void Up()
        {
            Sql(UpScript);
        }
        
        public override void Down()
        {
            Sql(DownScript);
        }

        private static string DownScript = "DROP procedure[dbo].[uspGetLeagueStats]";
        private static string UpScript = @"ALTER procedure [dbo].[uspGetLeagueStats] 
@leagueId varchar(255) 
as
DECLARE @stats TABLE 
  ( 
     playerid       UNIQUEIDENTIFIER, 
     playername     NVARCHAR(256), 
     gameweeknumber INT, 
     points         INT, 
     stat           NVARCHAR(64) 
  ) 
DECLARE @gameweekPointsData TABLE 
  ( 
     playerid       UNIQUEIDENTIFIER, 
     playername     NVARCHAR(256), 
     gameweeknumber INT, 
     gameweekpoints INT 
  ) 
DECLARE @gameweekFixturePoints TABLE 
  ( 
     playerid       UNIQUEIDENTIFIER, 
     playername     NVARCHAR(256), 
     gameweeknumber INT, 
     gameweekpoints INT 
  ) 

INSERT INTO @gameweekPointsData 
SELECT p.id           playerId, 
       p.NAME         AS PlayerName, 
       g.number       AS GameweekNumber, 
       Sum(pr.points) GameweekPoints 
FROM   player p 
       JOIN playerleagues pl 
         ON pl.playerid = p.id 
       JOIN league l 
         ON pl.leagueid = l.id 
       LEFT JOIN pick pic 
              ON pic.playerid = p.id 
       LEFT JOIN fixture f 
              ON pic.fixtureid = f.id 
       LEFT JOIN gameweek g 
              ON f.gameweekid = g.id 
       LEFT JOIN pickresult pr 
              ON pic.id = pr.pickid 
       LEFT JOIN competition c 
              ON l.competitionid = c.id 
WHERE  l.id = @leagueId 
GROUP  BY p.id, 
          p.NAME, 
          g.number 

INSERT INTO @gameweekFixturePoints 
            (playerid, 
             playername, 
             gameweeknumber, 
             gameweekpoints) 
SELECT player.id   PlayerId, 
       player.NAME PlayerName, 
       gw.number   GameweekNumber, 
       pr.points 
FROM   fixture f 
       JOIN result r 
         ON f.id = r.fixtureid 
       JOIN pick p 
         ON f.id = p.fixtureid 
       JOIN pickresult pr 
         ON p.id = pr.pickid 
       JOIN player player 
         ON player.id = p.playerid 
       JOIN gameweek gw 
         ON gw.id = f.gameweekid 
       JOIN playerleagues pl 
         ON pl.playerid = player.id 
       JOIN league l 
         ON pl.leagueid = l.id 
WHERE  l.id = @leagueId 

--Highest gameweek score 
INSERT INTO @stats 
            (playerid, 
             playername, 
             gameweeknumber, 
             points, 
             stat) 
SELECT playerid, 
       playername, 
       gameweeknumber, 
       gameweekpoints, 
       'HighestScore' 
FROM   @gameweekPointsData d 
       JOIN (SELECT Max(gameweekpoints) points 
             FROM   @gameweekPointsData) AS highest 
         ON highest.points = d.gameweekpoints 

--Lowest gameweek score 
INSERT INTO @stats 
            (playerid, 
             playername, 
             gameweeknumber, 
             points, 
             stat) 
SELECT playerid, 
       playername, 
       gameweeknumber, 
       gameweekpoints, 
       'LowestScore' 
FROM   @gameweekPointsData d 
       JOIN (SELECT Min(gameweekpoints) points 
             FROM   @gameweekPointsData) AS highest 
         ON highest.points = d.gameweekpoints 

--most correct scores in a week 
INSERT INTO @stats 
            (playerid, 
             playername, 
             gameweeknumber, 
             points, 
             stat) 
SELECT TOP 1 playerid, 
             playername, 
             gameweeknumber, 
             Count(*)            Points, 
             'MostCorrectInWeek' Stat 
FROM   @gameweekFixturePoints d 
WHERE  d.gameweekpoints >= 10 
GROUP  BY playerid, 
          playername, 
          gameweeknumber 
ORDER  BY points DESC 

--Highest scoring gameweek 
INSERT INTO @stats 
            (playerid, 
             playername, 
             gameweeknumber, 
             points, 
             stat) 
SELECT TOP 1 NULL                     PlayerId, 
             NULL                     PlayerName, 
             gameweeknumber, 
             Sum(gameweekpoints)      Points, 
             'HighestScoringGameweek' Stat 
FROM   @gameweekFixturePoints d 
GROUP  BY gameweeknumber 
ORDER  BY points DESC 

--Average score per week 
INSERT INTO @stats 
            (playerid, 
             playername, 
             gameweeknumber, 
             points, 
             stat) 
SELECT NULL                   PlayerId, 
       NULL                   PlayerName, 
       NULL                   GameweekNumber, 
       Avg(gameweekpoints)    Points, 
       'AverageGameweekScore' Stat 
FROM   (SELECT gameweeknumber, 
               Sum(gameweekpoints) GameweekPoints 
        FROM   @gameweekFixturePoints d 
        GROUP  BY gameweeknumber) a 

SELECT * 
FROM   @Stats 
GO
";
    }
}
