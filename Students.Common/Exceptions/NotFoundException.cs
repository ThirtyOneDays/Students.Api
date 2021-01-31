using System;

namespace Students.Common.Exceptions
{
  public class NotFoundException : Exception
  {
    public Type EntityType { get; }
    public object Id { get; }

    public NotFoundException(Type entityType, object id)
      : base($"The object of type {entityType.Name} is not found with id={id}")
    {
      EntityType = entityType;
      Id = id;
    }
  }
}
