using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Result : IModel
    {
        public Guid Id { get; set; }

        public Guid FixtureId { get; set; }

        [ForeignKey("FixtureId")]
        public virtual Fixture Fixture { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}
