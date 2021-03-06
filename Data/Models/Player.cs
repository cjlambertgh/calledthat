﻿using Data.DAL.Identity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Player: IModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int TotalScore { get; set; }

        public int GameWeekScore { get; set; }

        public bool EmailAlerts { get; set; }

        public virtual ICollection<PlayerLeagues> PlayerLeagues { get; set; }

        public virtual ICollection<LeagueOwners> LeagueOwners { get; set; }
    }
}
