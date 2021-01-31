using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Students.Common.Models.UI;
using Students.Repository.Filters;

namespace Students.Repository.Repositories
{
  public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
  {
    private readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;
    private readonly IDbFilter<TEntity> _dbFilter;

    public GenericRepository(ApplicationContext context, IDbFilter<TEntity> dbFilter)
    {
      _context = context;
      _dbSet = context.Set<TEntity>();
      _dbFilter = dbFilter;
    }

    public async Task CreateAsync(TEntity item)
    {
      await _dbSet.AddAsync(item);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity item)
    {
      _context.Entry(item).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }
    public async Task RemoveAsync(TEntity item)
    {
      _dbSet.Remove(item);
      await _context.SaveChangesAsync();
    }

    public async Task<TEntity> FindByIdAsync(long id)
    {
      return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(PagingModel pagingModel, string includeProperties = "")
    {
      IQueryable<TEntity> query = _dbSet;

      if (pagingModel.Filters != null && pagingModel.Filters.Any())
      {
        query = _dbFilter.Filter(pagingModel, query);
      }

      if (query.Any())
      {
        query = includeProperties
          .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
          .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
      }

      return await query.ToListAsync();
    }
  }
}
