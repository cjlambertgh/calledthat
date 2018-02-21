using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServices
{
    public interface IReminderService
    {
        void SendNewGameweekReminder(string url);
        void SendGameweekPicksNotEnteredReminder(string url);
    }
}
