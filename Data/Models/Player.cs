﻿using Data.DAL.Identity;
using Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Player: IModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public int TotalScore { get; set; }

        public int GameWeekScore { get; set; }
    }
}
