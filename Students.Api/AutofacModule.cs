using Autofac;
using Students.Logic.Services.Students;
using Students.Repository;
using Students.Repository.Entities;
using Students.Repository.Filters;
using Students.Repository.Interfaces;
using Students.Repository.Repositories;

namespace Students.Api
{
  public class AutofacModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterGeneric(typeof(GenericRepository<>))
        .As(typeof(IGenericRepository<>))
        .InstancePerLifetimeScope();

      builder.RegisterGeneric(typeof(DbFilter<>))
        .As(typeof(IDbFilter<>))
        .InstancePerLifetimeScope();

      builder.RegisterType<DbStudentsFilterAdapter>()
        .As<IDbFilterAdapter<DbStudent>>()
        .InstancePerLifetimeScope();

      builder.RegisterType<StudentsService>()
        .As<IStudentsService>()
        .InstancePerLifetimeScope();

      builder.RegisterType<UnitOfWork>()
        .As<IUnitOfWork>()
        .InstancePerLifetimeScope();

      builder.RegisterType<ApplicationContext>().AsSelf();
    }
  }
}
