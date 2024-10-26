using System;
using System.ComponentModel.DataAnnotations;

namespace CafeMenuProject.Models
{
    public class ExchangeRate
    {
        [Key]
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
