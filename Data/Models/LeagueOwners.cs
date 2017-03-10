using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class LeagueOwners: IModel
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }

        public Guid LeagueId { get; set; }

        [ForeignKey("LeagueId")]
        public virtual League League { get; set; }
    }
}
