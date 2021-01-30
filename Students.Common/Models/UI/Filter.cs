namespace Students.Common.Models.UI
{
  public class Filter
  {
    public string Column { get; set; }
    public FilterType Condition { get; set; }
    public string Value { get; set; }
  }
}
