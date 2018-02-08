using Data.Models;
using Data.Models.Procs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServices
{
    public interface IGameService
    {

        #region AdminMethods
        void Initialise();
        void UpdateApiData(string reminderEmailUrl);
        void UpdateResults();
        #endregion

        IEnumerable<Fixture> GetGameWeekFixtures();
        int GetCurrentGameweek();
        bool IsGameweekOpen(int gameweekNumber);
        bool IsCurrentGameweekOpen();
        void PopulatePickOpenCloseDates(out DateTime openDate, out DateTime closeDate);

        IEnumerable<Pick> GetPlayerPicks(Guid playerId, int gameweek);
        void AddPick(Guid playerId, Guid fixtureId, int homeScore, int awayScore, bool banker, bool doubleScore);

        IEnumerable<PlayerResults> GetPlayerResults(Guid playerId, int? gameWeek = null);
    }
}
