using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Students.Common.Models.UI;

namespace Students.Repository.Repositories
{
  public interface IGenericRepository<TEntity>
  {
    Task CreateAsync(TEntity item);
    Task UpdateAsync(TEntity item);
    Task RemoveAsync(TEntity item);
    Task<TEntity> FindByIdAsync(long id);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
    Task<IEnumerable<TEntity>> GetListAsync(PagingModel pagingModel, string includeProperties = "");
  }
}
