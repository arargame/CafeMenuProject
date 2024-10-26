using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using CafeMenuProject.Models;
using CafeMenuProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace CafeMenuProject.Services
{
    public class ExchangeRateService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;

        public ExchangeRateService(IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            _scopeFactory = scopeFactory;
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var rates = await context.ExchangeRates.ToDictionaryAsync(r => r.CurrencyCode, r => r.Rate);

            if (!rates.Any() || (DateTime.Now - context.ExchangeRates.Min(r => r.LastUpdated)).TotalHours >= 1)
            {
                await UpdateExchangeRatesAsync();
                rates = await context.ExchangeRates.ToDictionaryAsync(r => r.CurrencyCode, r => r.Rate);
            }

            return rates;
        }

        public async Task UpdateExchangeRatesAsync()
        {
            var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var response = await _httpClient.GetStringAsync(url);
            var xml = XDocument.Parse(response);

            var rates = new Dictionary<string, decimal>();

            foreach (XElement currency in xml.Descendants("Currency"))
            {
                var code = currency.Attribute("Kod")!.Value;
                var buyingRate = 1m / decimal.Parse(currency.Element("ForexBuying")!.Value, CultureInfo.InvariantCulture);
                rates[code] = buyingRate;
            }

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var rate in rates)
            {
                var existingRate = await context.ExchangeRates.FindAsync(rate.Key);
                if (existingRate == null)
                {
                    context.ExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyCode = rate.Key,
                        Rate = rate.Value,
                        LastUpdated = DateTime.Now
                    });
                }
                else
                {
                    existingRate.Rate = rate.Value;
                    existingRate.LastUpdated = DateTime.Now;
                }
            }

            await context.SaveChangesAsync();
        }

        public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency, Dictionary<string, decimal> rates)
        {
            if (fromCurrency == toCurrency) return amount;

            decimal fromRate = fromCurrency == "TRY" ? 1 : rates[fromCurrency];
            decimal toRate = toCurrency == "TRY" ? 1 : rates[toCurrency];

            return amount * (toRate / fromRate);
        }
    }
}
