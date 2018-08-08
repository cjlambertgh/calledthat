using FootballDataApiV2.Models;

namespace FootballDataApiV2.Interfaces
{
    public interface IMatchApi : IApi<Match>
    {
        int MatchDay { get; set; }
        int CompetitionId { get; set; }
    }
}
