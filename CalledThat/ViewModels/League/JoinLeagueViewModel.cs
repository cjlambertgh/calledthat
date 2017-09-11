using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalledThat.ViewModels.League
{
    public class JoinLeagueViewModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        [Display(Name = "Invite Code")]
        public string LeagueCode { get; set; }
    }
}