using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels.League
{
    public class ViewSingleLeagueViewModel
    {
        public string LeagueName { get; set; }
        public Guid LeagueId { get; set; }
        public List<LeagueTableRow> LeagueTableRows { get; set; }
        public bool IsLeagueOwner { get; set; }
        public string InviteCode { get; set; }

        public ViewSingleLeagueViewModel()
        {
            LeagueTableRows = new List<LeagueTableRow>();
        }
    }

    public class LeagueTableRow
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GameWeek { get; set; }
        public int  GameweekPoints { get; set; }
        public int TotalPoints { get; set; }
        public int LeaguePosition { get; set; }
    }
}