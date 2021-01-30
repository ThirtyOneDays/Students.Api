using System.Linq;
using Students.Common.Models.UI;
using Students.Repository.Entities;

namespace Students.Repository.Filters
{
  public class DbStudentsFilterAdapter : IDbFilterAdapter<DbStudent>
  {
    public IQueryable<DbStudent> FilterEqualsTo(IQueryable<DbStudent> dbStudents, Filter filter)
    {
      switch (filter.Column)
      {
        case nameof(DbStudent.Gender):
          dbStudents = dbStudents.Where(s => s.Gender.ToString() == filter.Value);
          break;
        case nameof(DbStudent.FirstName):
          dbStudents = dbStudents.Where(s => s.FirstName == filter.Value);
          break;
        case nameof(DbStudent.LastName):
          dbStudents = dbStudents.Where(s => s.LastName == filter.Value);
          break;
        case nameof(DbStudent.Patronymic):
          dbStudents = dbStudents.Where(s => s.Patronymic == filter.Value);
          break;
      }

      return dbStudents;
    }
  }
}
