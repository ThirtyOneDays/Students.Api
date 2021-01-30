using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Students.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var isService = args.Contains("--windows-service");

      IHostBuilder builder = CreateWebHostBuilder(args);

      if (isService)
      {
        builder.UseWindowsService();
      }
      var host = builder.Build();
      host.Run();
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        })
        .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
  }
}
