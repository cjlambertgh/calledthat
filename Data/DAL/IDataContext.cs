﻿using System.Data.Entity;
using Data.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data
{
    public interface IDataContext
    {
        DbSet<Competition> Competitions { get; set; }
        DbSet<Fixture> Fixtures { get; set; }
        DbSet<GameWeek> GameWeeks { get; set; }
        DbSet<LeagueOwners> LeagueOwners { get; set; }
        DbSet<League> Leagues { get; set; }
        DbSet<PickResult> PickResults { get; set; }
        DbSet<Pick> Picks { get; set; }
        DbSet<PlayerLeagues> PlayerLeagues { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<Result> Results { get; set; }
        DbSet<Season> Seasons { get; set; }
        DbSet<Team> Teams { get; set; }
        //List<T> SqlQuery<T>(string query, IEnumerable<SqlParameter> param);

        int SaveChanges();
    }
}