using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels.Game
{
    public class ResultsViewModel
    {
        public Dictionary<int, ResultItem> ResultItems { get; set; }
        public int Gameweek { get; set; }
        public int GameweekPoints { get; set; }

        public ResultsViewModel()
        {
            ResultItems = new Dictionary<int, ResultItem>();
        }

        public class ResultItem
        {
            public string HomeTeam { get; set; }
            public string AwayTeam { get; set; }
            public int HomeScore { get; set; }
            public int AwayScore { get; set; }
            public int HomePick { get; set; }
            public int AwayPick { get; set; }
            public int Points { get; set; }
        }
    }
}