using System.Collections.Generic;
using System.Threading.Tasks;
using Students.Common.Models.UI;

namespace Students.Repository.Repositories
{
  public interface IGenericRepository<TEntity>
  {
    Task Create(TEntity item);
    Task<IEnumerable<TEntity>> GetAsync(
      PagingModel pagingModel,
      string includeProperties = "");
  }
}
