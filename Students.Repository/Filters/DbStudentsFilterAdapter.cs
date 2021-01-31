using System.Linq;
using Students.Common.Models.UI;
using Students.Repository.Entities;

namespace Students.Repository.Filters
{
  public class DbStudentsFilterAdapter : IDbFilterAdapter<DbStudent>
  {
    public IQueryable<DbStudent> FilterEqualsTo(IQueryable<DbStudent> values, Filter filter)
    {
      values = filter.Column switch
      {
        nameof(DbStudent.Gender) => values.Where(s => s.Gender.ToString() == filter.Value),
        nameof(DbStudent.FirstName) => values.Where(s => s.FirstName == filter.Value),
        nameof(DbStudent.LastName) => values.Where(s => s.LastName == filter.Value),
        nameof(DbStudent.Patronymic) => values.Where(s => s.Patronymic == filter.Value),
        _ => values
      };

      return values;
    }
  }
}
