using System.Globalization;
using InventoryManagementSystem.Core.Models;
using InventoryManagementSystem.Services;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;
using Text = Spectre.Console.Text;

namespace InventoryManagementSystem;

public class App
{
    private readonly InventoryManager _inventoryManager;
    public App(InventoryManager inventoryManager)
    {
        _inventoryManager = inventoryManager;
    }
    public async Task Run(List<Product>? products)
    {
        await Loading();
        await AsciiText();
        await MenuList(products);

    }
    public async Task Loading(string action = "Loading...", string output = "Initializing...", int timer = 1000)
    {
        await Task.Delay(1000);
        AnsiConsole.Status()
        .Start(action, ctx =>
        {
            // Simulate some work       
            Thread.Sleep(timer);

            // Update the status and spinner
            ctx.Status(output);
            ctx.Spinner(Spinner.Known.Star2);
            ctx.SpinnerStyle(Style.Parse("cyan"));

            // Simulate some work
            Thread.Sleep(2000);
        });
    }

    public async Task AsciiText()
    {
        var intro = "Inventory Management System";
        await AsciiText(intro, "center"); // Use your overloaded method
    }
    public async Task AsciiText(string text, string justify)
    {
        var justified = justify.ToUpper();

        var asciiArt = new FigletText(text)
        {
            Color = Color.Cyan1,
            Justification = justified switch
            {
                "LEFT" => Justify.Left,
                "RIGHT" => Justify.Right,
                _ => Justify.Center
            }
        };

        AnsiConsole.Write(asciiArt);
    }


    public async Task MenuList(List<Product>? products)
    {
        //Menu Selection
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("[underline italic cyan]Inventory Menu[/]")
                .PageSize(10)
                .AddChoices(new[] {
                "Inventory List", "Add Product", "About",
                "Exit"
                }).WrapAround());


        if (selection == ("Inventory List"))
        {
            await InventoryList(products);
        }
        else if (selection == ("Add Product"))
        {
            await AddProduct(products);
        }
        else if (selection == ("About"))
        {
            await About(products);
        }
        else
        {
            var confirm = await MenuPrompt(products, "Do you want to close this program ?");
            if (confirm)
            {
                Environment.Exit(0);
            }
            else
            {
                await Refresh(products);
            }
        }
    }

    public async Task<string> AddQuestions(string question, string constraint)
    {
        var rule = new Rule($"[red]{constraint}[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);
        var answer = AnsiConsole.Prompt(
            new TextPrompt<string>($"{question}"));

        return answer;
    }

    public async Task Refresh(List<Product> products)
    {
        AnsiConsole.Clear();
        await Run(products);
    }


    public async Task<bool> MenuPrompt(List<Product> products, string message = "Do you want to go back in menu ?")
    {
        // Ask the user to confirm
        var confirmation = AnsiConsole.Prompt(
            new TextPrompt<bool>($"{message}")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "Y" : "N"));
        return confirmation;
    }


    #region UI-List
    public async Task InventoryList(List<Product> products)
    {
        //Module Action
        do
        {
            AnsiConsole.Clear();
            await AsciiText("Inventory List", "center");
            //Get Products Data
            var viewModel = await _inventoryManager.ListOfProducts(products);
            //Create Table
            var productsTable = new Table().Centered();

            //Add Default columns
            if (viewModel.Products != null)
            {
                if (viewModel != null && viewModel.Products.Any() && viewModel.Products.Count > 0)
                {
                    //Header
                    productsTable.AddColumn(new TableColumn("Id").Centered());
                    productsTable.AddColumn(new TableColumn("Name").Centered());
                    productsTable.AddColumn(new TableColumn("Quantity").Centered());
                    productsTable.AddColumn(new TableColumn("Price").Centered());
                    productsTable.AddColumn(new TableColumn("Total").Centered());
                    //Body
                    foreach (var p in viewModel.Products)
                    {
                        productsTable.AddRow(
                            p.ProductId.ToString(),
                            p.Name.ToString(),
                            p.QuantityInStock.ToString(),
                            p.Price.ToString(),
                            (p.QuantityInStock * p.Price).ToString()
                            );
                    }
                    //Footer
                    productsTable.AddEmptyRow().AddRow(
                            "",
                            "",
                            "",
                            "Total : ",
                            viewModel.Products.Sum(p => p.Total.GetValueOrDefault()).ToString()
                            );
                }
            }
            else
            {
                productsTable.AddColumn(new TableColumn("No Data to show").Centered());
            }

            AnsiConsole.Write(productsTable);




            var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("[underline italic cyan]Action Menu[/]")
                .PageSize(10)
                .AddChoices(new[] {
                "Add Product", "Edit Product", "Remove Product", "Back to Menu"
                }).WrapAround());

            // Handle action selection
            if (action == "Back to Menu")
            {
                var confirm = await MenuPrompt(products);
                if (confirm)
                {
                    await Refresh(products);
                }
            }
            else
            {
                // Handle "Add Product", "Edit Product" or "Remove Product" actions here
                if (action == "Add Product")
                {
                    await AddProduct(products);
                }
                else if (action == "Edit Product")
                {
                    if (products != null)
                    {
                        var edit = await MenuPrompt(products, "Do you want to edit product?");
                        if (edit)
                        {
                            Console.WriteLine("Enter the product ID to edit: ");
                            var id = Console.ReadLine();
                            id = (id?.Trim());
                            if (int.TryParse(id, out int productId)){}
                            var data = await _inventoryManager.GetById(productId, products);
                            await EditProduct(data, products);
                        }
                    }
                    else
                    {
                        AnsiConsole.Clear();
                        await AsciiText("No products available to edit.", "center");
                        var confirm = await MenuPrompt(products, "Do you want to go back in Inventory Menu ?");
                        if (confirm)
                        {
                            await InventoryList(products);
                        }
                        else
                        {
                            await Refresh(products);
                        }
                        break;
                    }
                }
                else if (action == "Remove Product")
                {
                    // Code to remove the product
                }
            }

        } while (true);
    }

    public async Task AddProduct(List<Product> products)
    {
        do
        {

            AnsiConsole.Clear();
            await AsciiText("Add Product", "left");

            //Menu Prompt
            var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("[underline italic cyan]Action Menu[/]")
                .PageSize(10)
                .AddChoices(new[] {
                "Add Product", "Inventory List", "Back to Menu"
                }).WrapAround());

            // Handle action selection
            if (action == "Back to Menu")
            {
                var confirm = await MenuPrompt(products);
                if (confirm)
                {
                    await Refresh(products);
                }
            }
            else if (action == "Inventory List")
            {
                await InventoryList(products);
            }
            else
            {
                //Add product
                var name = await AddQuestions("Product Name : ", "Name must be unique");
                _inventoryManager.NameChecker(name, products);
                var quantity = await AddQuestions("Quantity : ", "Quantity must be a positive integer");
                //Parse the quantity to decimal and check if the inputed is a non-negative number
                if (decimal.TryParse(quantity, out decimal quantityInstock))
                {
                    if (quantityInstock <= 0.00m)
                    {
                        AnsiConsole.MarkupLine("[red]Quantity must be greater than 0.00[/]");
                        continue;
                    }
                }
                else
                {  
                    AnsiConsole.MarkupLine("[red]Invalid number format.[/]");
                    continue;
                }

                var price = await AddQuestions("Price : ", "Price must be a positive integer");
                //Parse the price to decimal and check if the inputed is a non-negative number
                if (decimal.TryParse(price, out decimal priceInStock))
                {
                    if (priceInStock <= 0.00m)
                    {
                        AnsiConsole.MarkupLine("[red]Price must be greater than 0.00[/]");
                        continue;
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid number format.[/]");
                    continue;
                }

                products = await _inventoryManager.AddProduct(name, quantityInstock, priceInStock, products);


                await Loading("Creating", "Saving", 500);
                //await Refresh(products);
            }


        } while (true);

    }


    public async Task EditProduct(Product product, List<Product> productlist)
    {
        do
        {

            AnsiConsole.Clear();
            await AsciiText("Edit Product", "left");

            //Layout
            // Create the layout
            var layout = new Layout("Root")
                .SplitColumns(
                    new Layout("Right"));

            // Update the left column
            layout["Right"].Update(
            new Panel(
                Align.Center(
                    new Markup($"\n Id \n [blue]{product.ProductId}[/] \n " +
                    $"Name \n [blue]{product.Name}[/] \n" +
                    $"Quantity \n [blue]{product.QuantityInStock}[/] \n" +
                    $"Price \n [blue]{product.Price}[/] \n" +
                    $"Total \n [blue]{product.Total}[/] \n" +
                    $"").Justify(Justify.Center),
                    VerticalAlignment.Middle))
                .Padding(new Padding (2,2,2,2))
                );


            // Render the layout
            AnsiConsole.Write(layout);

            //Menu Prompt
            var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .Title("[underline italic cyan]Action Menu[/]")
                .PageSize(10)
                .AddChoices(new[] {
                "Edit This Product", "Inventory List", "Back to Menu"
                }).WrapAround());

            // Handle action selection
            if (action == "Back to Menu")
            {
                var confirm = await MenuPrompt(productlist);
                if (confirm)
                {
                    await Refresh(productlist);
                }
            }
            else if (action == "Inventory List")
            {
                await InventoryList(productlist);
            }
            else
            {
                //Edit product
                var name = await AddQuestions("Product Name : ", "Name must be unique");
                _inventoryManager.NameChecker(name, productlist);
                var quantity = await AddQuestions("Quantity : ", "Quantity must be a positive integer");
                //Parse the quantity to decimal and check if the inputed is a non-negative number
                if (decimal.TryParse(quantity, out decimal quantityInstock))
                {
                    if (quantityInstock <= 0.00m)
                    {
                        Console.WriteLine("Quantity must be greater than 0.00");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number format.");
                    Environment.Exit(0);
                }

                var price = await AddQuestions("Price : ", "Price must be a positive integer");
                //Parse the price to decimal and check if the inputed is a non-negative number
                if (decimal.TryParse(price, out decimal priceInStock))
                {
                    if (priceInStock <= 0.00m)
                    {
                        Console.WriteLine("Price must be greater than 0.00");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number format.");
                    Environment.Exit(0);
                }

                productlist = await _inventoryManager.AddProduct(name, quantityInstock, priceInStock, productlist);


                await Loading("Updating", "Saving", 500);
                //await Refresh(products);
            }


        } while (true);

    }

    public async Task About(List<Product> products)
    {
        //About UI
        do
        {
            AnsiConsole.Clear();
            await AsciiText("About Me", "left");
            var table = new Table().RightAligned().NoBorder();
            table.AddColumn("Created By Jericho Mosqueda");
            AnsiConsole.Write(table);

            var confirm = await MenuPrompt(products);
            if (confirm)
            {
                await Refresh(products);
            }
        } while (true);
    }

    #endregion
}