﻿using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class League : IModel
    {
        public Guid Id { get; set; }

        public Guid CompetitionId { get; set; }

        [ForeignKey("CompetitionId")]
        public virtual Competition Competition { get; set; }

        public string Name { get; set; }

        public string InviteCode { get; set; }
    }
}
