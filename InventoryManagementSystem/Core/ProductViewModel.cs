using InventoryManagementSystem.Core.Models;

namespace InventoryManagementSystem.Core;

public class ProductViewModel
{
    public List<Product>? Products { get; set; } = new List<Product>();
    public decimal? TotalValue { get; set; }
}
