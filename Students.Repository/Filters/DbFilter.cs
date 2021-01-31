using System.Linq;
using Students.Common.Models.UI;

namespace Students.Repository.Filters
{
  public class DbFilter<T> : IDbFilter<T> where T : class
  {
    private readonly IDbFilterAdapter<T> _adapter;

    private const int DefaultFrom = 0;
    private const int DefaultTo = 20;

    public DbFilter(IDbFilterAdapter<T> adapter)
    {
      _adapter = adapter;
    }

    public IQueryable<T> Filter(PagingModel pagingModel, IQueryable<T> values)
    {
      foreach (var filter in pagingModel.Filters)
      {
        if (filter.Condition == FilterType.EqualsTo)
        {
          values = _adapter.FilterEqualsTo(values, filter);
        }
      }

      if (pagingModel.Paging.From < 0)
      {
        pagingModel.Paging.From = DefaultFrom;
      }
      if (pagingModel.Paging.To <= 0)
      {
        pagingModel.Paging.To = DefaultTo;
      }

      values = values
        .Skip(pagingModel.Paging.From)
        .Take(pagingModel.Paging.To);

      return values;
    }
  }
}
