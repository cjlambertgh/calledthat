/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  StoredProcedure [dbo].[uspGetLeagueTable]    Script Date: 23/09/2017 12:06:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE procedure [dbo].[uspGetLeagueTable] 
@leagueId varchar(255) 
as
SELECT
   p.name PlayerName,
   p.Id PlayerId,
   COALESCE(GameWeek.GameweekNumber, 1) GameweekNumber,
   l.Name LeagueName,
   COALESCE(Total.TotalPoints, 0) TotalPoints,
   COALESCE(Gameweek.GameweekPoints , 0) GameweekPoints
FROM
   (
      SELECT
         p.Id playerId,
         SUM(pr.Points) TotalPoints 
      FROM
         player p 
         JOIN
            PlayerLeagues pl 
            ON pl.PlayerId = p.Id 
         JOIN
            League l 
            ON pl.LeagueId = l.Id 
         LEFT JOIN
            Pick pic 
            ON pic.PlayerId = p.Id 
         LEFT JOIN
            PickResult pr 
            ON pic.id = pr.PickId 
      WHERE
         l.id = @leagueId
      GROUP BY
         p.id 
   )
   AS Total 
   Left JOIN
      (
         SELECT
            p.Id playerId,
			g.Number as GameweekNumber,
            SUM(pr.Points) GameweekPoints 
         FROM
            player p 
            JOIN
               PlayerLeagues pl 
               ON pl.PlayerId = p.Id 
            JOIN
               League l 
               ON pl.LeagueId = l.Id 
            LEFT JOIN
               Pick pic 
               ON pic.PlayerId = p.Id 
            LEFT JOIN
               Fixture f 
               ON pic.FixtureId = f.Id 
            LEFT JOIN
               GameWeek g 
               ON f.GameWeekId = g.Id 
            LEFT JOIN
               PickResult pr 
               ON pic.id = pr.PickId 
            LEFT JOIN
               Competition c 
               ON l.CompetitionId = c.Id 
         WHERE
            l.id = @leagueId 
			AND g.number =  c.CurrentGameWeekNumber
         GROUP BY
            p.id ,
			g.Number
      )
      AS Gameweek 
      ON Total.playerId = Gameweek.playerId 
   JOIN
      Player p 
      ON p.id = Total.playerId
	JOIN
		PlayerLeagues pl 
		ON pl.PlayerId = p.Id 
	JOIN
		League l 
		ON pl.LeagueId = l.id
	WHERE l.id = @leagueId


