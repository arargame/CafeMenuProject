using System.Collections.Generic;

namespace CafeMenuProject.ViewModels
{
    public class DashboardViewModel
    {
        public List<CategoryProductCountViewModel> CategoryProductCounts { get; set; }
        public Dictionary<string, decimal> ExchangeRates { get; set; }
    }

    public class CategoryProductCountViewModel
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }
}
