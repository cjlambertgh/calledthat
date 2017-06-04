using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Pick : IModel
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public Guid FixtureId { get; set; }

        [ForeignKey("FixtureID")]
        public Fixture Fixture { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public bool Banker { get; set; }

        public bool Double { get; set; }
    }
}
