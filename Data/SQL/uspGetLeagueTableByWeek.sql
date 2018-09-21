CREATE procedure [dbo].[uspGetLeagueTableByWeek] 
@leagueId varchar(255),
@week int
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
		LEFT JOIN
               Fixture f 
               ON pic.FixtureId = f.Id 
        LEFT JOIN
            GameWeek g 
            ON f.GameWeekId = g.Id
		LEFT JOIN
            GameWeek gwstart 
            ON l.GameweekIdScoringStarts = gwstart.Id 
      WHERE
         l.id = @leagueId
		 and (l.GameweekIdScoringStarts is null or g.Number >= gwstart.Number)
		 and g.number <= @week
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
               PickResult pr 
               ON pic.id = pr.PickId 
            LEFT JOIN
               Competition c 
               ON l.CompetitionId = c.Id
			LEFT JOIN
               GameWeek g 
               ON f.GameWeekId = g.Id  and c.Id = g.CompetitionId
         WHERE
            l.id = @leagueId 
			AND g.number =  @week 
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


GO


