using EmailService;
using FakeItEasy;
using GameServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Data.Models;
using Data.DAL.Identity;

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

        [TestMethod]
        public void PicksNotEntered_GameweekClosed_DoesntSend()
        {
            A.CallTo(() => _gameService.IsCurrentGameweekOpen()).Returns(false);
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts()).Returns(new List<string>());
            A.CallTo(() => _gameService.GetCurrentGameweek()).Returns(1);
            A.CallTo(() => _gameService.GetAllPlayerPicks(1)).Returns(new List<Pick>());
            var svc = CreateService();

            svc.SendGameweekPicksNotEnteredReminder("");

            A.CallTo(() => _gameEmailService.SendPicksNotEnteredEmail(null, null)).WithAnyArguments()
                .MustNotHaveHappened();
        }

        [TestMethod]
        public void PicksNotEntered_NoEmailsEnabled_DoesntSend()
        {
            A.CallTo(() => _gameService.IsCurrentGameweekOpen()).Returns(true);
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts()).Returns(new List<string>());
            A.CallTo(() => _gameService.GetCurrentGameweek()).Returns(1);
            A.CallTo(() => _gameService.GetAllPlayerPicks(1)).Returns(new List<Pick>());
            var svc = CreateService();

            svc.SendGameweekPicksNotEnteredReminder("");

            A.CallTo(() => _gameEmailService.SendPicksNotEnteredEmail(null, null)).WithAnyArguments()
                .MustNotHaveHappened();
        }

        [TestMethod]
        public void PicksNotEntered_OneEmailEnabled_NoMissingPicks_DoesntSend()
        {

            var email = "a@example.com";
            var appUser = A.Fake<AppUser>();
            A.CallTo(() => appUser.Email).Returns(email);
            var pick = new Pick
            {
                Player = new Player
                {
                    AppUser = appUser
                }
            };
            A.CallTo(() => _gameService.IsCurrentGameweekOpen()).Returns(true);
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts())
                .Returns(new List<string> { email });
            A.CallTo(() => _gameService.GetCurrentGameweek()).Returns(1);
            A.CallTo(() => _gameService.GetAllPlayerPicks(1)).Returns(new List<Pick> { pick });
            var svc = CreateService();

            svc.SendGameweekPicksNotEnteredReminder("");

            A.CallTo(() => _gameEmailService.SendPicksNotEnteredEmail(null, null)).WithAnyArguments()
                .MustNotHaveHappened();
        }

        [TestMethod]
        public void PicksNotEntered_MissingPicks_EmailsEnabled_DoesSend()
        {
            var emailEnabled = "a@example.com";
            var emailMissed = "b@example.com";
            var url = "www.example.com";
            var appUser = A.Fake<AppUser>();
            A.CallTo(() => appUser.Email).Returns(emailMissed);
            var pick = new Pick
            {
                Player = new Player
                {
                    AppUser = appUser
                }
            };
            A.CallTo(() => _gameService.IsCurrentGameweekOpen()).Returns(true);
            A.CallTo(() => _playerService.GetPlayersEmailsAcceptedAlerts())
                .Returns(new List<string> { emailEnabled });
            A.CallTo(() => _gameService.GetCurrentGameweek()).Returns(1);
            A.CallTo(() => _gameService.GetAllPlayerPicks(1)).Returns(new List<Pick> { pick });
            var svc = CreateService();

            svc.SendGameweekPicksNotEnteredReminder(url);

            A.CallTo(() => _gameEmailService.SendPicksNotEnteredEmail(emailEnabled, url))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

    }
}
