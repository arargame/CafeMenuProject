using System.Collections.Generic;
using CafeMenuProject.Models;

namespace CafeMenuProject.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public string SelectedCurrency { get; set; }
    }
}
