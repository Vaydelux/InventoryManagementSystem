namespace InventoryManagementSystem.Core.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal QuantityInStock { get; set; }
    public decimal Price { get; set; }

    #region Display Properties
    public decimal? Total { get; set; }
    #endregion

}
