using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Portfolio.API.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
