using System;

namespace Students.Common.Exceptions
{
  public class DuplicateGroupNameException : Exception
  {
    public DuplicateGroupNameException() : base("Group with the same name already exists")
    {
    }
  }
}
