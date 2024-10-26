using CafeMenuProject.Models;

namespace CafeMenuProject.ViewModels
{
    public class CustomerMenuViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public Dictionary<string, decimal> ExchangeRates { get; set; }
        public string SelectedCurrency { get; set; }
    }
}
