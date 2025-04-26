using InventoryManagementSystem;
using InventoryManagementSystem.Core.Models;
using InventoryManagementSystem.Services;

class Program
{
    static async Task Main (string[] args)
    {
        //Initialize the app with the InventoryManager
        var app = new App(new InventoryManager());
        //Run App.cs
        await app.Run(new List<Product>());
    }
}