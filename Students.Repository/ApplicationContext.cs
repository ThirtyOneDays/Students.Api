using Microsoft.EntityFrameworkCore;
using Students.Repository.Entities;

namespace Students.Repository
{
  public sealed class ApplicationContext : DbContext
  {
    public DbSet<DbStudent> Students { get; set; }
    public DbSet<DbGroup> Groups { get; set; }

    private readonly string _connectionString;

    public ApplicationContext(DbConnectionSettings connectionStringSettings)
    {
      _connectionString = connectionStringSettings.ConnectionString;
      Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<DbStudent>().HasKey(s => s.Id);
      modelBuilder.Entity<DbStudent>().Property(s => s.Gender).IsRequired();
      modelBuilder.Entity<DbStudent>().Property(s => s.LastName).IsRequired();
      modelBuilder.Entity<DbStudent>().Property(s => s.FirstName).IsRequired();
      modelBuilder.Entity<DbStudent>().Property(s => s.Patronymic);
      modelBuilder.Entity<DbStudent>().HasIndex(s => s.UId).IsUnique();
      modelBuilder.Entity<DbStudent>().Property(s => s.Gender).IsRequired();
      modelBuilder.Entity<DbStudent>()
        .HasMany(s => s.Group)
        .WithMany(c => c.Student)
        .UsingEntity(j => j.ToTable("StudentGroups"));

      modelBuilder.Entity<DbGroup>().HasKey(s => s.Id);
      modelBuilder.Entity<DbGroup>().HasIndex(s => s.Name).IsUnique();
    }
  }
}
