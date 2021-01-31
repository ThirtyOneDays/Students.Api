using System.Linq;
using Students.Common.Models.UI;
using Students.Repository.Entities;

namespace Students.Repository.Filters
{
  public class DbUsersFilterAdapter : IDbFilterAdapter<DbUser>
  {
    public IQueryable<DbUser> FilterEqualsTo(IQueryable<DbUser> values, Filter filter)
    {
      values = filter.Column switch
      {
        nameof(DbUser.UserName) => values.Where(s => s.UserName == filter.Value),
        _ => values
      };

      return values;
    }
  }
}
