namespace Syncball.Core.Data.Repositories;

using Syncball.Core.Data.Models;

public interface ICountryRepository : IRepository<Country>
{
    Task<bool> ExistsAsync(string countryName);
    Task<int> EnableCountryAsync(int id);
    Task<int> DisableCountryAsync(int id);
    Task<IEnumerable<Country>> GetAllEnabledAsync();
}