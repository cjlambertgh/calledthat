﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public interface IGameService
    {

        #region AdminMethods
        void Initialise();
        void UpdateApiData();
        void UpdateResults();
        #endregion

        IEnumerable<Fixture> GetGameWeekFixtures();
        int GetCurrentGameweek();
        bool IsGameweekOpen(int gameweekNumber);
        bool IsCurrentGameweekOpen();

        IEnumerable<Pick> GetPlayerPicks(Guid playerId, int gameweek);
        void AddPick(Guid playerId, Guid fixtureId, int homeScore, int awayScore, bool banker, bool doubleScore);

        
    }
}
