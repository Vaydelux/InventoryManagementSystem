using InventoryManagementSystem;
using InventoryManagementSystem.Services;

class Program
{
    static async Task Main (string[] args)
    {
        var app = new App(new InventoryManager());

        await app.Run(null);
    }
}