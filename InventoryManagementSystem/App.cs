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

    public async Task Loading()
    {
        await Task.Delay(1000);
        AnsiConsole.Status()
        .Start("Loading...", ctx =>
        {
            // Simulate some work
            Thread.Sleep(1000);

            // Update the status and spinner
            ctx.Status("Initializing...");
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


        AnsiConsole.Clear();

        if (selection == ("Inventory List"))
        {
            await Run(products);
        }
        else if (selection == ("Add Product"))
        {
            await AsciiText("Add Product", "left");
            var name = await AddQuestions("Product Name : ", "Name must be unique");
            _inventoryManager.NameChecker(name, products);
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

            await _inventoryManager.AddProduct(name, quantityInstock, priceInStock, products);


        }
        else if (selection == ("About"))
        {
            await AsciiText("About Me", "left");
            var table = new Table().RightAligned().NoBorder();
            table.AddColumn("Created By Jericho Mosqueda");
            AnsiConsole.Write(table);
            Console.WriteLine("Do you want to go back to the menu? (Y/N)");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Y)
            {
                AnsiConsole.Clear();
                await Run(products);
            }
            else if (key == ConsoleKey.N)
            {
                AnsiConsole.MarkupLine("[red]Exiting...[/]");
                Environment.Exit(0);
            }
            else
            {
                AnsiConsole.Clear();
                await Run(products);
            }
        }
        else
        {
            // Ask the user to confirm
            var confirmation = AnsiConsole.Prompt(
                new TextPrompt<bool>("Do you want to close this program?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "Y" : "N"));

            if (confirmation)
            {
                AnsiConsole.MarkupLine("[red]Exiting...[/]");
                Environment.Exit(0);
            }
            else
            {
                AnsiConsole.Clear();
                await Run(products);
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

}
