using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CafeMenuProject.Services
{
    public class ExchangeRateUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ExchangeRateUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var exchangeRateService = scope.ServiceProvider.GetRequiredService<ExchangeRateService>();
                    await exchangeRateService.UpdateExchangeRatesAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
