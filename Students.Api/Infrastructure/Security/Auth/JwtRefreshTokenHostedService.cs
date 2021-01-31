using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Students.Api.Infrastructure.Security.Auth
{
  public class JwtRefreshTokenHostedService : IHostedService, IDisposable
  {
    private Timer _timer;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JwtRefreshTokenHostedService(IJwtAuthManager jwtAuthManager, IServiceScopeFactory serviceScopeFactory)
    {
      _jwtAuthManager = jwtAuthManager;
      _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      using var scope = _serviceScopeFactory.CreateScope();
      _timer = new Timer(DoWork, null, TimeSpan.FromMinutes(15), Timeout.InfiniteTimeSpan);
      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      _jwtAuthManager.RemoveExpiredRefreshTokens(DateTime.Now);
      _timer?.Change(TimeSpan.FromMinutes(15), Timeout.InfiniteTimeSpan);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _timer?.Change(Timeout.Infinite, 0);
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}
