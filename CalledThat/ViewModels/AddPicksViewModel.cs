using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels
{
    public class AddPicksViewModel
    {
        public Guid PlayerId { get; set; }

        public List<PickItem> PickItems { get; set; } = new List<PickItem>();

        public bool ReadOnly { get; set; }

        public int Gameweek { get; set; }

        public DateTime OpenDateTime { get; set; }

        public DateTime CloseDateTime { get; set; }
    }

    public class PickItem
    {
        public Guid FixtureId { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public string HomeTeamBadgeUrl { get; set; }

        public string AwayTeamBadgeUrl { get; set; }

        public DateTime KickOffTime { get; set; }

        [Required(ErrorMessage ="Enter a score")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a number")]
        public string HomeScore { get; set; }

        [Required(ErrorMessage = "Enter a score")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a number")]
        public string AwayScore { get; set; }

        public bool Banker { get; set; }

        public bool Double { get; set; }

        
    }
}