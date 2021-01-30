using System.Collections.Generic;

namespace Students.Common.Models.UI
{
  public class PagingModel
  {
    public List<Filter> Filters { get; set; }
    public Paging Paging { get; set; }
  }
}
