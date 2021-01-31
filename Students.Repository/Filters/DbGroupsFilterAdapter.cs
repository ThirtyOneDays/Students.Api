using System.Linq;
using Students.Common.Models.UI;
using Students.Repository.Entities;

namespace Students.Repository.Filters
{
  public class DbGroupsFilterAdapter : IDbFilterAdapter<DbGroup>
  {
    public IQueryable<DbGroup> FilterEqualsTo(IQueryable<DbGroup> values, Filter filter)
    {
      values = filter.Column switch
      {
        nameof(DbStudent.Gender) => values.Where(s => s.Name == filter.Value),
        _ => values
      };

      return values;
    }
  }
}
