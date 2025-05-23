namespace SPInRepositoryPro.Repository
{
    public interface IGenericRepository<T> where T : class
    {

        Task<int> GetCount(string SearchTerm);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(string SearchTerm, int? PageNumber, int PageSize, string SortColumn, string SortDirection);
        Task<T> GetByIdAsync(object id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task SaveAsync();
    }
}
