using System.Linq;
using Students.Common.Models.UI;

namespace Students.Repository.Filters
{
  public interface IDbFilterAdapter<T>
  {
    IQueryable<T> FilterEqualsTo(IQueryable<T> values, Filter filter);
  }
}
