using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeMenuProject.Models;
using CafeMenuProject.Services;
using CafeMenuProject.Data;
using CafeMenuProject.ViewModels;

namespace CafeMenuProject.Controllers;

public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ExchangeRateService _exchangeRateService;

    public CustomerController(ApplicationDbContext context, ExchangeRateService exchangeRateService)
    {
        _context = context;
        _exchangeRateService = exchangeRateService;
    }

    public async Task<IActionResult> Menu(int? categoryId, string currency = "TRY")
    {
        var query = _context.Categories.Where(c => !c.IsDeleted);
        
        if (categoryId.HasValue)
        {
            query = query.Where(c => c.CategoryId == categoryId.Value);
        }

        var categories = await query
            .AsNoTracking()
            .Include(c => c.Products.Where(p => !p.IsDeleted))
            .ToListAsync();

        var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();

        var viewModel = new CustomerMenuViewModel
        {
            Categories = categories,
            ExchangeRates = exchangeRates,
            SelectedCurrency = currency
        };

        foreach (var category in viewModel.Categories)
        {
            foreach (var product in category.Products)
            {
                product.Price = _exchangeRateService.ConvertCurrency(product.Price, product.Currency, currency, exchangeRates);
                product.Currency = currency;
            }
        }

        return View(viewModel);
    }
}
