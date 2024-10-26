using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeMenuProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatorUserId { get; set; }
        public string Currency { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductProperty> ProductProperties { get; set; } = [];

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
