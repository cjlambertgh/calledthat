using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels.League
{
    public class CreateLeagueViewModel
    {
        public string LeagueName { get; set; }
        public Dictionary<Guid, string> Competitions { get; set; }
        public Guid SelectedCompetitionId { get; set; }
    }
}