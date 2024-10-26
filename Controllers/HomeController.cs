using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CafeMenuProject.Models;
using CafeMenuProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CafeMenuProject.Data;
using CafeMenuProject.ViewModels;

namespace CafeMenuProject.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ExchangeRateService _exchangeRateService;

    public HomeController(ApplicationDbContext context, ExchangeRateService exchangeRateService)
    {
        _context = context;
        _exchangeRateService = exchangeRateService;
    }

    public async Task<IActionResult> Index(string currency = "TRY")
    {
        var products = await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .ToListAsync();

        var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();

        foreach (var product in products)
        {
            product.Price = _exchangeRateService.ConvertCurrency(product.Price, product.Currency, currency, exchangeRates);
            product.Currency = currency;
        }

        var viewModel = new HomeViewModel
        {
            Products = products,
            SelectedCurrency = currency
        };

        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Dashboard()
    {
        var categoryProductCounts = await _context.Categories
            .Select(c => new CategoryProductCountViewModel
            {
                CategoryName = c.CategoryName,
                ProductCount = c.Products.Count()
            })
            .ToListAsync();

        var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();

        var viewModel = new DashboardViewModel
        {
            CategoryProductCounts = categoryProductCounts,
            ExchangeRates = exchangeRates
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetExchangeRates()
    {
        var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();
        return Json(exchangeRates);
    }
}
