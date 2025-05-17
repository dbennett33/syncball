namespace Syncball.Core.Data.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> UpsertAsync(T entity);
    Task<int> DeleteAsync(T entity);
}
