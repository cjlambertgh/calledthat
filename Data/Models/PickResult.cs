using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PickResult: IModel
    {
        public Guid Id { get; set; }

        public Guid PickId { get; set; }

        [ForeignKey("PickId")]
        public Pick Pick { get; set; }

        public int Points { get; set; }

    }
}
