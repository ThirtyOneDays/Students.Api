using Microsoft.EntityFrameworkCore;
using Students.Repository.Entities;

namespace Students.Repository
{
  public sealed class ApplicationContext : DbContext
  {
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbGroup> Groups { get; set; }

    public ApplicationContext()
    {
      Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=studentsdb;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<DbStudent>().Property(s => s.Gender).IsRequired();
      modelBuilder.Entity<DbStudent>().Property(s => s.LastName).IsRequired();
      modelBuilder.Entity<DbStudent>().Property(s => s.FirstName).IsRequired();
      modelBuilder.Entity<DbStudent>().Property(s => s.Patronymic);
      modelBuilder.Entity<DbStudent>().HasIndex(s => s.UId).IsUnique();
      modelBuilder.Entity<DbStudent>().Property(s => s.Gender).IsRequired();

      modelBuilder.Entity<DbGroup>().Property(s => s.Name).IsRequired();
    }
  }
}
