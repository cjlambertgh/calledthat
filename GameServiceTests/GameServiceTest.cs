using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Interfaces;
using Data.Models;
using GameServices;
using FakeItEasy;
using Data.Repository;
using System.Collections.Generic;
using FootballDataApiV2.Interfaces;

namespace GameServiceTests
{
    [TestClass]
    public class GameServiceTest
    {
        private IDataContextConnection _dataContextConnection;
        private IReminderService _reminderService;
        private ICompetitionApi _competitionApi;
        private ITeamApi _teamApi;
        private IMatchApi _matchApi;

        public GameServiceTest()
        {
            _dataContextConnection = A.Fake<IDataContextConnection>();
            _reminderService = A.Fake<IReminderService>();
            _teamApi = A.Fake<ITeamApi>();
            _competitionApi = A.Fake<ICompetitionApi>();
            _matchApi = A.Fake<IMatchApi>();
        }
        private GameService CreateService()
        {
            return new GameService(_dataContextConnection, _reminderService, _competitionApi, _teamApi, _matchApi);
        }

        [TestMethod, TestCategory("ServiceInstantiation")]
        public void Can_Create_Service()
        {
            var svc = CreateService();

            Assert.IsNotNull(svc);
            Assert.IsInstanceOfType(svc, typeof(GameService));
        }

        [TestMethod, TestCategory("Initialise")]
        public void Initialise_Existing_Competitions_Does_Nothing()
        {
            A.CallTo(() => _dataContextConnection.Database.Competitions.Any(null)).WithAnyArguments().Returns(true);
            var svc = CreateService();

            A.CallTo(() => _dataContextConnection.Database.SaveChanges()).MustNotHaveHappened();
        }

        [TestMethod, TestCategory("GetPlayerPicks")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPlayerPicks_Null_PlayerId_Throws()
        {
            var svc = CreateService();

            svc.GetPlayerPicks(Guid.Empty, 0);
        }

        [TestMethod, TestCategory("GetPlayerPicks")]
        public void GetPlayerPicks_Enumerable_Of_Picks()
        {
            A.CallTo(() => _dataContextConnection.Database.Picks.Get(null, null)).WithAnyArguments().Returns(new List<Pick>());

            var svc = CreateService();
            var result = svc.GetPlayerPicks(Guid.NewGuid(), 0);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Pick>));
        }

        [TestMethod, TestCategory("GetCurrentGameweek")]
        public void GetCurrentGameweek_Null_Gameweek_Returns_One()
        {
            A.CallTo(() => _dataContextConnection.Database.Competitions.FirstOrDefault(null)).Returns(null);

            var svc = CreateService();
            var result = svc.GetCurrentGameweek();

            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);
        }

        [TestMethod, TestCategory("GetCurrentGameweek")]
        public void GetCurrentGameweek_Returns_GameweekNumber()
        {
            var comp = new Competition { CurrentGameWeekNumber = 5 };
            A.CallTo(() => _dataContextConnection.Database.Competitions.FirstOrDefault(null)).Returns(comp);

            var svc = CreateService();
            var result = svc.GetCurrentGameweek();

            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(5, result);
        }
    }
}
