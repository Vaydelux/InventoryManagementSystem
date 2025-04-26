using InventoryManagementSystem.Core.Models;

namespace InventoryManagementSystem.Core;

public class ProductViewModel
{
    public List<Product>? Products { get; set; }
    public decimal? TotalValue { get; set; }
}
