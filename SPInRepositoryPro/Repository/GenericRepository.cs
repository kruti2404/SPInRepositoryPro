using SPInRepositoryPro.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using SPInRepositoryPro.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPInRepositoryPro.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ProgramDbContext _context;
        private readonly DbSet<T> table;

        public GenericRepository(ProgramDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }


        public async Task<int> GetCount(string SearchTerm)
        {
            var employees = await _context.Employees.FromSqlInterpolated($"EXEC SP_GetEmployeeCount {SearchTerm ?? (object)DBNull.Value}").ToListAsync();

            int Count = employees.Count();


            return Count;

        }



        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync(string SearchTerm = "", int? PageNumber = 1, int PageSize= 10,  string SortColumn= "Id", string SortDirection= "ASC")
        {
            var employees = _context.Employees.FromSqlInterpolated($"Exec Sp_SearchSortPag {PageNumber}, {PageSize}, {SearchTerm??""},{SortColumn}, {SortDirection} ").AsEnumerable();
            return (IEnumerable<T>)employees;
        }


        public async Task<T> GetByIdAsync(object id)
        {
            return await table.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await table.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await table.FindAsync(id);
            if (entity != null)
            {
                table.Remove(entity);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
