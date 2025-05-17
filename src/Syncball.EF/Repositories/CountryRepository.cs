namespace Syncball.EF.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncball.Core.Data.Models;
using Syncball.Core.Data.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly SyncballContext _context;
    private readonly ILogger<CountryRepository> _logger;

    public CountryRepository(SyncballContext context, ILogger<CountryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> DeleteAsync(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
    
        if (country.Id <= 0)
        {
            throw new ArgumentException("Entity must have a valid ID to be deleted", nameof(country));
        }
    
        try
        {            
            var exists = await _context.Countries.AnyAsync(c => c.Id == country.Id);
            if (!exists)
            {
                _logger.LogWarning("Attempted to delete non-existent country with ID: {Id}", country.Id);
                return 0;
            }
        
            _context.Countries.Remove(country);
            return 1; 
        }    
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting country with ID: {Id}", country.Id);
            return -1;
        }
    }

    public async Task<int> DisableCountryAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid country ID for disabling: {Id}", id);
            return -1;
        }

        try
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                _logger.LogWarning("Attempted to disable non-existent country with ID: {Id}", id);
                return 0;
            }

            if (!country.Enabled)
            {
                return 0;
            }

            country.Enabled = false;
            country.LastUpdated = DateTime.UtcNow;
            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling country with ID: {Id}", id);
            return -1;
        }
    }

    public async Task<int> EnableCountryAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid country ID for enabling: {Id}", id);
            return -1;
        }

        try
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                _logger.LogWarning("Attempted to enable non-existent country with ID: {Id}", id);
                return 0;
            }

            if (country.Enabled)
            {
                return 0;
            }

            country.Enabled = true;
            country.LastUpdated = DateTime.UtcNow;
            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling country with ID: {Id}", id);
            return -1;
        }
    }

    public async Task<bool> ExistsAsync(string countryName)
    { 
        ArgumentNullException.ThrowIfNullOrEmpty(countryName, nameof(countryName));

        try
        {
            return await _context.Countries.AnyAsync(c => c.Name == countryName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if country exists with name: {CountryName}", countryName);
            return false; 
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid country ID for existence check: {Id}", id);
            return false;
        }

        try
        {
            return await _context.Countries.AnyAsync(c => c.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if country exists with ID: {Id}", id);
            return false;
        }
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        try
        {
            return await _context.Countries.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all countries");
            return [];
        }
    }

    public async Task<IEnumerable<Country>> GetAllEnabledAsync()
    {
        try
        {
            return await _context.Countries
                .Where(c => c.Enabled)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving enabled countries");
            return [];
        }
    }

    public async Task<Country?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Invalid country ID: {Id}", id);
            return null;
        }

        try
        {
            return await _context.Countries.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving country with ID: {Id}", id);
            return null;
        }
    }

    public async Task<int> UpsertAsync(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);

        try
        {
            var existingCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Name == country.Name);
            if (existingCountry != null)
            {
                country.Id = existingCountry.Id;
                country.LastUpdated = DateTime.UtcNow;
                _context.Entry(existingCountry).CurrentValues.SetValues(country);
                return existingCountry.Id;
            }
            else
            {
                country.LastUpdated = DateTime.UtcNow;
                await _context.Countries.AddAsync(country);
                return 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error upserting country: {CountryName}", country.Name);
            return -1;
        }
    }


}
