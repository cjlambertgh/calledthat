using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class GameWeek : IModel
    {
        public Guid Id { get; set; }

        public Guid CompetitionId { get; set; }

        [ForeignKey("CompetitionId")]
        public virtual Competition Competition { get; set; }

        public int Number { get; set; }

        public DateTime PickOpenDateTime { get; set; }

        public DateTime PickCloseDateTime { get; set; }

        public virtual ICollection<Fixture> Fixtures { get; set; }
    }
}
