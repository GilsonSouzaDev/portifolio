using System.Linq.Expressions;

namespace Portfolio.API.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    Task<T?> GetByIdAsync(int id);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
