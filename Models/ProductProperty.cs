namespace CafeMenuProject.Models;

public class ProductProperty
{
    public int ProductPropertyId { get; set; }
    public int ProductId { get; set; }
    public int PropertyId { get; set; }

    public virtual Product Product { get; set; }
    public virtual Property Property { get; set; }
}
