/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  StoredProcedure [dbo].[uspGetPlayerResults]    Script Date: 23/09/2017 12:07:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[uspGetPlayerResults] 
@playerId varchar(255) 
as

select pl.Name PlayerName, gw.Number GameweekNumber, f.KickOffDateTime, home.name HomeTeam, away.name AwayTeam, 
r.HomeScore, r.AwayScore, p.HomeScore HomeScorePick, p.AwayScore AwayScorePick, pr.Points, p.Banker, p.[Double],
home.BadgeUrl HomeBadgeUrl, away.BadgeUrl AwayBadgeUrl
from Fixture f
join team home on f.HomeTeamId = home.Id
join team away on f.AwayTeamId = away.Id
join Result r on f.Id = r.FixtureId
join Pick p on f.Id = p.FixtureId
join PickResult pr on p.id = pr.PickId
join Player pl on pl.Id = p.PlayerId
join GameWeek gw on gw.Id = f.GameWeekId
where pl.Id = @playerId
order by gw.Number,f.KickOffDateTime



