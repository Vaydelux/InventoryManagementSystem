using InventoryManagementSystem.Core;
using InventoryManagementSystem.Core.Models;
using Spectre.Console;
using System.Xml.Linq;

namespace InventoryManagementSystem.Services;

public class InventoryManager
{
    #region Methods

    public async Task<List<Product>> AddProduct(string name, decimal quantity, decimal price, List<Product>? products)
    {
        products ??= new List<Product>(); // Initialize if null
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
            if (data != null)
                products.Remove(data);
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[red]Item already removed.[/]");
        }

        return products;
    }

    public async Task<ProductViewModel?> ListOfProducts(List<Product>? products)
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
        if (products.Any() && products.Count > 0)
        {
            var lastProduct = products.OrderBy(x => x.ProductId).Last();
            return lastProduct.ProductId + 1;
        }
        else
        {
            return 1;
        }
    }

    public async Task<bool> NameChecker(string name, int id, List<Product>? products)
    {
        if (products != null)
        {
            // Check if the name already exists in the list of products
            var data = products.FirstOrDefault(x => x.Name.Trim().ToLower() == name.Trim().ToLower());
            if(data != null)
            {
                if (data.ProductId != id || id == 0)
                {
                    AnsiConsole.MarkupLine("[red]Product name already exists.[/]");
                    return true;
                }
            }
            
        }
        return false;
    }

    public async Task<Product> GetById(int id, List<Product>? products)
    {
        var data = products.FirstOrDefault(x => x.ProductId == id);
        if (data == null)
        {
            AnsiConsole.MarkupLine("[red]Product with this Id does not exist.[/]");
        }
        return data;
    }

    #endregion


}
