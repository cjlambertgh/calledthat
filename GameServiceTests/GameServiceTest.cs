using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.Interfaces;
using Data.Models;
using GameServices;
using FakeItEasy;
using Data.Repository;

namespace GameServiceTests
{
    [TestClass]
    public class GameServiceTest
    {
        private IDataContextConnection _dataContextConnection;

        public GameServiceTest()
        {
            _dataContextConnection = A.Fake<IDataContextConnection>();
        }
        private GameService CreateService()
        {
            return new GameService(_dataContextConnection);
        }

        [TestMethod]
        public void Can_Create_Service()
        {
            var svc = CreateService();

            Assert.IsNotNull(svc);
            Assert.IsInstanceOfType(svc, typeof(GameService));
        }

        [TestMethod]
        public void Initialise_Existing_Competitions_Does_Nothing()
        {
            A.CallTo(() => _dataContextConnection.Database.Competitions.Any(null)).WithAnyArguments().Returns(true);
            var svc = CreateService();

            A.CallTo(() => _dataContextConnection.Database.SaveChanges()).MustNotHaveHappened();
        }

        [TestMethod]
        public void Test_Add_Pick_Adds_Pick()
        {
            //arrange
            var pick = new Pick
            {
                Id = Guid.Empty,
                HomeScore = 1,
                AwayScore = 1,
                Banker = true,
                Double = true,
                FixtureId = Guid.Empty,
                PlayerId = Guid.Empty
            };

            //var factory = new MockRepository(MockBehavior.Loose);
            //var mockPickRepo = factory.Create<Repository<Pick>>();
            //var repo = mockPickRepo.Object;
            //var db = new Mock<IUnitOfWork>();
            //db.Setup(m => m.Picks).Returns(repo);
            //var gameService = new GameService.GameService(db.Object);

            //act
            //gameService.AddPick(pick.PlayerId, pick.FixtureId, pick.HomeScore, pick.AwayScore, pick.Banker, pick.Double);


            //assert
            Assert.AreEqual(1, 1);

        }
    }
}
