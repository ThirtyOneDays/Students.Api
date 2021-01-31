using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Students.Api.Extensions;
using Students.Api.Middlewares;
using Students.Logic.Services.Students;

namespace Students.Api
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvcCore(opt => opt.EnableEndpointRouting = false);
      services.AddHttpContextAccessor()
        .AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(StudentsMappingProfile)))
        .AddMvcCore(opt => opt.EnableEndpointRouting = false)
        .AddDataAnnotations()
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
          options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
        })
        .AddApiExplorer();

      services.AddCustomSwaggerGen();
      services.AddCustomAuthorizations();
      services.AddCustomJwtAuthentication(_configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMiddleware<ErrorHandlingMiddleware>();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Students Api V1");
      });
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
      app.UseMvc();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      builder.RegisterModule(new AutofacModule(_configuration));
    }
  }
}
