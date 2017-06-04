using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public interface IGameService
    {
        void Initialise();
        void UpdateAll();

        IEnumerable<Fixture> GetGameWeekFixtures();
        IEnumerable<Pick> GetPlayerPicks(Guid playerId, int gameweek);

        void AddPick(Guid playerId, Guid fixtureId, int homeScore, int awayScore, bool banker, bool doubleScore);
    }
}
