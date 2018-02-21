using EmailService;
using FakeItEasy;
using GameServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServiceTests
{
    [TestClass]
    public class ReminderServiceTests
    {
        IGameEmailService _gameEmailService = A.Fake<IGameEmailService>();
        IPlayerService _playerService = A.Fake<IPlayerService>();
        IGameService _gameService = A.Fake<IGameService>();

        private ReminderService CreateService()
        {
            var svc = new ReminderService(_gameEmailService, _playerService, _gameService);
            return svc;
        }

        [TestMethod]
        public void CanCreateService()
        {
            var svc = CreateService();

            Assert.IsInstanceOfType(svc, typeof(ReminderService));
            Assert.IsNotNull(svc);
        }

        [TestMethod]
        public void NoPlayers_NoEmailsTriggered()
        {
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts()).Returns(new List<string>());
            var svc = CreateService();

            svc.SendNewGameweekReminder("testurl");

            A.CallTo(() => _gameEmailService.SendGameweekOpenEmail(null, null)).WithAnyArguments().MustNotHaveHappened();
        }

        [TestMethod]
        public void OnePlayer_SingleEmailTriggered()
        {
            var url = "testUrl";
            var email = "a@example.com";
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts()).Returns(new List<string>
            {
                email
            });
            var svc = CreateService();

            svc.SendNewGameweekReminder(url);

            A.CallTo(() => _gameEmailService
            .SendGameweekOpenEmail(email, url))
            .MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void TwoPlayers_TwoEmailsTriggered()
        {
            var url = "testUrl";
            var email1 = "a@example.com";
            var email2 = "b@example.com";
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts()).Returns(new List<string>
            {
                email1,
                email2
            });
            var svc = CreateService();

            svc.SendNewGameweekReminder(url);

            A.CallTo(() => _gameEmailService.SendGameweekOpenEmail(email1, url))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _gameEmailService.SendGameweekOpenEmail(email2, url))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _gameEmailService.SendGameweekOpenEmail(null, null))
                .WithAnyArguments().MustHaveHappened(Repeated.Exactly.Twice);
        }
    }
}
