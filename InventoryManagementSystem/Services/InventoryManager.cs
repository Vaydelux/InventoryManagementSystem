using InventoryManagementSystem.Core;
using InventoryManagementSystem.Core.Models;
using System.Xml.Linq;

namespace InventoryManagementSystem.Services;

public class InventoryManager
{
    #region Methods

    public async Task<List<Product>> AddProduct(string name, decimal quantity, decimal price, List<Product>? products)
    {
        products.Add(new Product
        {
            ProductId = IdGenerator(products),
            Name = name,
            QuantityInStock = quantity,
            Price = price,
            Total = quantity * price,
        });

        return products;
    }
    public async Task<List<Product>> UpdateProduct(int id, string name, decimal quantity, decimal price, List<Product>? products)
    {
        var data = products.FirstOrDefault(x => x.ProductId == id);
        if (data != null)
        {
            data.Name = name;
            data.QuantityInStock = quantity;
            data.Price = price;
            data.Total = price * quantity;
        }

        return products;
    }
    public async Task<List<Product>> RemoveProduct(int id, List<Product>? products)
    {
        try
        {
            var data = products.FirstOrDefault(x => x.ProductId == id);
            if(data != null)
                products.Remove(data);
        }
        catch (Exception)
        {

            Console.WriteLine("Item already removed");
        }

        return products;
    }

    public async Task<ProductViewModel> ListOfProducts(List<Product>? products)
    {
        var productViewModel = new ProductViewModel();
        try
        {
            productViewModel.Products = products;
            productViewModel.TotalValue = GetTotalValue(products);
            return productViewModel;
        }
        catch (Exception)
        {

            Console.WriteLine("No Products found");
            Environment.Exit(0);
            return productViewModel;
        }
    }

    public decimal GetTotalValue(List<Product> products)
    {
        var total = products.Sum(p => p.Total);
        return total ?? 0.00m;
    }


    public int IdGenerator(List<Product>? products)
    {
        if (products.Any() & products.Count() > 0)
        {
            var lastProduct = products.OrderBy(x => x.ProductId).Last();
            return lastProduct.ProductId + 1;
        }
        else
        {
            return 1;
        }
    }

    public void NameChecker(string name, List<Product>? products)
    {
        if (products != null)
        {
            var data = products.Any(x => x.Name == name);
            if (data)
            {
                Console.WriteLine("Product name already exists");
                Environment.Exit(0);
            }
        }
    }

    #endregion


}
