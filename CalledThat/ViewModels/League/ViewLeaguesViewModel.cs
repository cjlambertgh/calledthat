using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels.League
{
    public class ViewLeaguesViewModel
    {
        public List<LeagueItem> LeagueItems { get; set; }

        public ViewLeaguesViewModel()
        {
            LeagueItems = new List<LeagueItem>();
        }        
    }

    public class LeagueItem
    {
        public Guid LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string Competition { get; set; }
        public bool LeagueOwner { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}