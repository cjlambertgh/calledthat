/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  StoredProcedure [dbo].[uspGetLeagueTable]    Script Date: 11/01/2018 22:26:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[uspGetLeagueStats] 
@leagueId varchar(255) 
as
DECLARE @stats TABLE (
  Playerid uniqueidentifier,
  PlayerName nvarchar(256),
  GameweekNumber int,
  Points int,
  Stat nvarchar(64)
)

DECLARE @data TABLE (
  playerId uniqueidentifier,
  PlayerName nvarchar(256),
  GameweekNumber int,
  GameweekPoints int
)

INSERT INTO @data
  SELECT
    p.Id playerId,
	p.Name AS PlayerName,
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
  GROUP BY p.id, p.Name,
           g.Number


INSERT INTO @stats (PlayerId, PlayerName, GameweekNumber, Points, Stat)
  SELECT
    playerId,
	PlayerName,
    GameWeekNumber,
    GameweekPoints,
    'HighestScore'
  FROM @data d
  JOIN (SELECT
    MAX(GameweekPoints) points
  FROM @data) AS highest
    ON highest.points = d.GameweekPoints

INSERT INTO @stats (PlayerId, PlayerName, GameweekNumber, Points, Stat)
  SELECT
    playerId,
	PlayerName,
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