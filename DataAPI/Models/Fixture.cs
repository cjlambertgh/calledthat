using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class FixtureWrapper : IModel
    {
        public List<Fixture> Fixtures { get; set; }
    }
    public class Fixture: IModel
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public string Date { get; set; }
        public int MatchDay { get; set; }
        public string HomeTeamName { get; set; }
        public int HomeTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public int AwayTeamId { get; set; }
        public Result Result { get; set; }
    }

}
