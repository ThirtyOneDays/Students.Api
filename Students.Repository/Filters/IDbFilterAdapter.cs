using System.Linq;
using Students.Common.Models.UI;
using Students.Repository.Entities;

namespace Students.Repository.Filters
{
  public interface IDbFilterAdapter<in T>
  {
    IQueryable<DbStudent> FilterEqualsTo(IQueryable<T> dbStudents, Filter filter);
  }
}
