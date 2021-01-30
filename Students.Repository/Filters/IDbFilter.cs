using System.Linq;
using Students.Common.Models.UI;

namespace Students.Repository.Filters
{
  public interface IDbFilter<T>
  {
    IQueryable<T> Filter(PagingModel pagingModel, IQueryable<T> values);
  }
}
