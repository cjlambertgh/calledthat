using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Competition : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid SeasonId { get; set; }

        [ForeignKey("SeasonId")]
        public virtual Season Season { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<GameWeek> GameWeeks { get; set; }

        public virtual ICollection<League> Leagues { get; set; }

    }
}
