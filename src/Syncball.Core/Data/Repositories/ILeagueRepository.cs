namespace Syncball.Core.Data.Repositories;

using Syncball.Core.Data.Models;

public interface ILeagueRepository : IRepository<League>
{
    Task<IEnumerable<League>> GetLeaguesByCountryIdAsync(int countryId);
    Task<int> EnableLeagueAsync(int id);
    Task<int> DisableLeagueAsync(int id);
    Task<IEnumerable<League>> GetAllEnabledAsync();
}
