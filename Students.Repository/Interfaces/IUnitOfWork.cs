using System;
using System.Threading.Tasks;
using Students.Repository.Entities;
using Students.Repository.Repositories;

namespace Students.Repository.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IGenericRepository<DbUser> UserRepository { get; }
    IGenericRepository<DbStudent> StudentRepository { get; }
    IGenericRepository<DbGroup> GroupRepository { get; }
    Task SaveAsync();
  }
}
