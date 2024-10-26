using CafeMenuProject.Models;

namespace CafeMenuProject.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatorUserId { get; set; }

    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> ChildCategories { get; set; } = [];
    public virtual ICollection<Product> Products { get; set; } = [];
    public DateTime UpdatedDate { get; set; }
    public string? UpdaterUserId { get; set; }
}
