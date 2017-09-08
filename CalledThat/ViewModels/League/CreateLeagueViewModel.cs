using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels.League
{
    public class CreateLeagueViewModel
    {
        [Required]
        [MinLength(3)]
        [Display(Name = "League Name")]
        public string LeagueName { get; set; }
        
        [Required]
        [Display(Name = "Competition")]
        public Guid SelectedCompetitionId { get; set; }

        public Dictionary<Guid, string> Competitions { get; set; }
    }
}