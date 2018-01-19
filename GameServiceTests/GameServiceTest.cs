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
    }
}
