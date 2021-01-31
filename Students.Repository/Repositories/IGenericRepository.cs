using System.Collections.Generic;
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
    Task<IEnumerable<TEntity>> GetAsync(
      PagingModel pagingModel,
      string includeProperties = "");
  }
}
