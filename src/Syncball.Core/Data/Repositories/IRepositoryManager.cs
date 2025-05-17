namespace Syncball.Core.Data.Repositories;

public interface IRepositoryManager
{
    ICountryRepository CountryRepository { get; }
    ILeagueRepository LeagueRepository { get; }

    Task<int> CompleteAsync();
    Task<int> BeginTransactionAsync();
    Task<int> CommitTransactionAsync();
    Task<int> RollbackTransactionAsync();
    Task<int> MigrateAsync();
}
