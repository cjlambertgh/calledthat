using FootballDataApiV2.Models;

namespace FootballDataApiV2.Interfaces
{
    public interface ITeamApi : IApi<Team>
    {
        int CompetitionId { get; set; }
    }
}
