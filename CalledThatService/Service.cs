using Data.DAL;
using EmailService;
using GameService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CalledThatService
{
    public partial class Service : ServiceBase
    {
        private Timer timer;
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer
            {
                Interval = 1000 * 60 * 60,
            };

            timer.Elapsed += new ElapsedEventHandler(TimerOnTick);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        private void TimerOnTick(object sender, ElapsedEventArgs e)
        {
            try
            {
                //var db = new DataContextConnection();
                //var _playerService = new PlayerService(db);
                //var gameweekOpenEmailRecipients = _playerService.GetPlayersEmailsAcceptedAlerts();
                //var _gameEmailService = new GameEmailService(new SmtpMailService());
                //var gs = new GameService.GameService(db, _gameEmailService);
                //Action gameweekUpdatedAction = () =>
                //{
                //    Parallel.ForEach(gameweekOpenEmailRecipients, (address) =>
                //    {
                //        _gameEmailService.SendGameweekOpenEmail(address, Url.Action("Index", "Home", null, Request.Url.Scheme));
                //    });
                //};
                //gs.UpdateApiData(gameweekUpdatedAction);
                //gs.UpdateResults();
            }
            catch(Exception ex)
            {
                var mailSvc = new SmtpMailService();
                mailSvc.Send("phateuk@Gmail.com", "CalledThat Service Exception", ex.ToString());
            }
            
        }
    }
}
