using Students.Repository.Repositories;
using System;
using System.Threading.Tasks;
using Students.Repository.Entities;

namespace Students.Repository.Interfaces
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ApplicationContext _context;

    public UnitOfWork(
      ApplicationContext context,
      IGenericRepository<DbUser> userRepository,
      IGenericRepository<DbStudent> studentRepository,
      IGenericRepository<DbGroup> groupRepository)
    {
      _context = context;
      UserRepository = userRepository;
      StudentRepository = studentRepository;
      GroupRepository = groupRepository;
    }

    public IGenericRepository<DbUser> UserRepository { get; }

    public IGenericRepository<DbStudent> StudentRepository { get; }

    public IGenericRepository<DbGroup> GroupRepository { get; }

    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }
      _disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
