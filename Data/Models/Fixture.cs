using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Fixture : IModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid HomeTeamId { get; set; }

        [ForeignKey("HomeTeamId")]
        public virtual Team HomeTeam { get; set; }

        public Guid AwayTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public virtual Team AwayTeam { get; set; }

        public Guid GameWeekId { get; set; }

        [ForeignKey("GameWeekId")]
        public virtual GameWeek GameWeek { get; set; }

        public DateTime KickOffDateTime { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public Fixture()
        {
            Results = new List<Result>();
        }
    }
}
