using Autofac;
using Microsoft.Extensions.Configuration;
using Students.Logic;
using Students.Logic.Services.Groups;
using Students.Logic.Services.Students;
using Students.Logic.Services.Users;
using Students.Repository;
using Students.Repository.Entities;
using Students.Repository.Filters;
using Students.Repository.Interfaces;
using Students.Repository.Repositories;

namespace Students.Api
{
  public class AutofacModule : Module
  {
    private readonly IConfiguration _configuration;

    public AutofacModule(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
      builder.Register(c => new DbConnectionSettings
      {
        ConnectionString = _configuration["Database:ConnectionString"],
      }).As<DbConnectionSettings>().SingleInstance();

      builder.RegisterGeneric(typeof(GenericRepository<>))
        .As(typeof(IGenericRepository<>))
        .InstancePerLifetimeScope();

      builder.RegisterGeneric(typeof(DbFilter<>))
        .As(typeof(IDbFilter<>))
        .InstancePerLifetimeScope();

      builder.RegisterType<DbUsersFilterAdapter>()
        .As<IDbFilterAdapter<DbUser>>()
        .InstancePerLifetimeScope();

      builder.RegisterType<DbStudentsFilterAdapter>()
        .As<IDbFilterAdapter<DbStudent>>()
        .InstancePerLifetimeScope();

      builder.RegisterType<DbGroupsFilterAdapter>()
        .As<IDbFilterAdapter<DbGroup>>()
        .InstancePerLifetimeScope();

      builder.RegisterType<UsersService>()
        .As<IUsersService>()
        .InstancePerLifetimeScope();

      builder.RegisterType<StudentsService>()
        .As<IStudentsService>()
        .InstancePerLifetimeScope();

      builder.RegisterType<GroupsService>()
        .As<IGroupsService>()
        .InstancePerLifetimeScope();

      builder.RegisterType<UnitOfWork>()
        .As<IUnitOfWork>()
        .InstancePerLifetimeScope();

      builder.RegisterType<PasswordHasher>()
        .As<IPasswordHasher>()
        .InstancePerLifetimeScope();

      builder.RegisterType<ApplicationContext>().AsSelf();
    }
  }
}
