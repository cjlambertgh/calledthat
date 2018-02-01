using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalledThat.ViewModels.League
{
    public class ManageLeagueViewModel
    {
        public Data.Models.League League { get; set; }
        public Dictionary<Guid, int> GameWeeks { get; set; }

        public Guid SelectedStartWeek { get; set; }
        public IEnumerable<SelectListItem> AvailableGameweeks { get; set; }
    }
}