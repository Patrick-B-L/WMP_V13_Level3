
// Main program

using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Reflection;

// Greet the user
Display.Greeting();
// Add sample data to the list of assets
Listmanager.InitializeSampleData();

// A loop displaying the main menu with three choices: Show list of assets, add asset or exit the application
while (true)
{
    Display.MainMenu();

    

    int userOption = UserInputValidation.GetValidMainMenuOption();

    switch (userOption)
    {
        case 1:
            {
                Listmanager.ShowElectronicAssets();
                break;
            }
        case 2:
            {
                AssetManager.AddAsset();
                break;
            }
        case 3:
            { 
                Environment.Exit(0);
                break;
            }
    }
}

// Base class. Handles properties and methods for selecting a country and connect the selection to the matching currency and exchange rate
class OfficeCountry
{
    public OfficeCountry(string country, string currency, decimal exchangeRate)
    {
        Country = country;
        Currency = currency;
        ExchangeRate = exchangeRate;
    }

    public string Country { get; set; }
    public string Currency { get; set; }
    public decimal ExchangeRate { get; set; }

    public static OfficeCountry SelectCountry()
    {
        Console.WriteLine("Select a Country:");
        Console.WriteLine("(1) Sweden");
        Console.WriteLine("(2) United Kingdom");
        Console.WriteLine("(3) USA");

        int typeOption = UserInputValidation.GetValidCountryMenuOption();

        switch (typeOption)
        {
            case 1:
                return new OfficeCountry("Sweden", "SEK", 10);
            case 2:
                return new OfficeCountry("United Kingdom", "GBP", 0.8m);
            default:
                return new OfficeCountry("USA", "USD", 1);
        }
    }
}
// Extends OfficeCountry with properties and methods for electronics. 
class Electronics : OfficeCountry
{
    public Electronics(string country, string electronicType, string brand, string model, DateOnly purchaseDate, decimal price, string currency, decimal exchangeRate)
        : base(country, currency, exchangeRate) 
    {
        ElectronicType = electronicType;
        Brand = brand;
        Model = model;
        PurchaseDate = purchaseDate;
        Price = price;
    }

    public string ElectronicType { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public decimal Price { get; set; }

}

// Extends Electronics with properties and methods for computers. 
class Computer : Electronics
{
    public Computer(string country, string electronicType, string brand, string model, DateOnly purchaseDate, decimal price, string currency, decimal exchangeRate)
        :base(country, electronicType, brand, model, purchaseDate, price, currency, exchangeRate)
    {
    }
}

// Extends Electronics with properties and methods for phones. 
class Phone : Electronics
{
    public Phone(string country, string electronicType, string brand, string model, DateOnly purchaseDate, decimal price, string currency, decimal exchangeRate)
        : base(country, electronicType, brand, model, purchaseDate, price, currency, exchangeRate)
    { 
    }
    
}

// Includes methods for managing lists, formatted printing of the list and sample data
class Listmanager
{
    // Instatiates a list with properties from the Electronics class
    private static List<Electronics> electronicsList = new List<Electronics>();

    public static List<Electronics> sortedByClassAndPurchaseDateElectronicsList()
    {
        return electronicsList.OrderBy(asset => asset.Country).ThenBy(asset => asset.PurchaseDate).ToList();
    }
    // Show a diagram of the assets 
    public static void ShowElectronicAssets()
    {
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        // Show header 
        Console.WriteLine("Type".PadRight(20) + "Brand".PadRight(20) + "Model".PadRight(20) + "Office".PadRight(20) + "Purchase Date".PadRight(20) + "Price in USD".PadRight(20) + "Currency".PadRight(20) + "Local Price Today");
        Console.WriteLine("----".PadRight(20) + "-----".PadRight(20) + "-----".PadRight(20) + "------".PadRight(20) + "-------------".PadRight(20) + "------------".PadRight(20) + "--------".PadRight(20) + "-----------------");
        // Show assets
        foreach (var asset in sortedByClassAndPurchaseDateElectronicsList())
        {
            // Highlight the row as yellow if the asset is less 6 months from its' end of life (36 months) and red if the asset is less than 3 months from its' end of life
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int monthsLeft = (36 - ((today.Year - asset.PurchaseDate.Year) * 12 + (today.Month - asset.PurchaseDate.Month)));
            // Decrease monthsLeft by 1 if the day of the month for the asset's PurchaseDatethe is less than the day of month for today  
            if (asset.PurchaseDate.Day < today.Day)
            {
                monthsLeft--;
            }
            if (monthsLeft < 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (monthsLeft < 6)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            // If more than 3 months left to the asset's end of life than just show the row in nomal text color
            else
            {
                Console.ResetColor();
            }
            Console.WriteLine(asset.ElectronicType.PadRight(20) + asset.Brand.PadRight(20) + asset.Model.PadRight(20) + asset.Country.PadRight(20) + asset.PurchaseDate.ToString().PadRight(20) + asset.Price.ToString().PadRight(20) + asset.Currency.PadRight(20) + (asset.Price * asset.ExchangeRate));
            Console.ResetColor();
        }
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
    }

    public static void AddElectronicAsset(Electronics asset)
    {
        electronicsList.Add(asset);
    }

    public static void InitializeSampleData()
    {
        electronicsList.Add(new Computer("Sweden", "Computer", "Dell", "XPS 13", new DateOnly(2022, 1, 15), 1200, "SEK", 10));
        electronicsList.Add(new Computer("United Kingdom", "Computer", "HP", "Spectre x360", new DateOnly(2021, 5, 20), 1500, "GBP", 0.8m));
        electronicsList.Add(new Phone("USA", "Phone", "Apple", "iPhone 13", new DateOnly(2023, 3, 10), 999, "USD", 1));
        electronicsList.Add(new Phone("Sweden", "Phone", "Samsung", "Galaxy S21", new DateOnly(2022, 7, 25), 850, "SEK", 10));
        electronicsList.Add(new Computer("United Kingdom", "Computer", "Acer", "Swift 5", new DateOnly(2023, 3, 12), 1100, "GBP", 0.8m));
        electronicsList.Add(new Phone("USA", "Phone", "Google", "Pixel 7", new DateOnly(2023, 10, 5), 899, "USD", 1));
        electronicsList.Add(new Computer("Sweden", "Computer", "Lenovo", "ThinkPad X1 Carbon", new DateOnly(2022, 6, 15), 1400, "SEK", 10));
        electronicsList.Add(new Phone("USA", "Phone", "Samsung", "Galaxy S22", new DateOnly(2023, 7, 20), 999, "USD", 1));
        electronicsList.Add(new Computer("United Kingdom", "Computer", "Apple", "MacBook Pro 14", new DateOnly(2021, 9, 10), 1800, "GBP", 0.8m));
        electronicsList.Add(new Phone("Sweden", "Phone", "OnePlus", "10T", new DateOnly(2023, 2, 14), 750, "SEK", 10));

    }
}

// Includes methods for validating user inputs

class UserInputValidation
{


    public static string GetValidBrand()
    {
        while (true)
        {
            // Error handling with try-catch block
            try
            {
                Console.Write("Enter a Brand: ");
                string brand = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(brand))
                {
                    throw new ArgumentException("Brand can't be empty. Please try again!");
                }
                else
                {
                    return brand;
                }
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }

    public static string GetValidModel()
    {
        while (true)
        {
            Console.Write("Enter a Model: ");
            string model = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(model))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Model can't be empty. Please try again!");
                Console.ResetColor();
                
            }
            else
            {
                return model;
            }
        }
    }

    public static DateOnly GetValidDate()
    {
        while (true)
        {
            Console.Write("Enter a Purchase Date (In the format Year, Month, Day): ");
            if (DateOnly.TryParse(Console.ReadLine().Trim(), out DateOnly purchaseDate))
            {
                return purchaseDate;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Purchase Date. Please try again!");
                Console.ResetColor();
                
            }
        }
    }

    public static decimal GetValidPrice()
    {
        while (true)
        {
            Console.Write("Enter a Price: ");
            if (decimal.TryParse(Console.ReadLine().Trim(), out decimal price) && price > 0)
            {
                return price;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Price. Please try again!");
                Console.ResetColor(); 
            }
        }
    }

    public static void InvalidOption()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid option. Please try again!");
        Console.ResetColor();
    }
    public static int GetValidMainMenuOption()
    {
        while (true)
        {
            Console.WriteLine("Enter an option:");
            if (int.TryParse(Console.ReadLine().Trim(), out int option) && option > 0 && option <= 3)
            {
                return option;
            }
            else
            {
                InvalidOption();
            }
        }
    }

    public static int GetValidTypeMenuOption()
    {
        while (true)
        {
            Console.WriteLine("Enter an option:");
            if (int.TryParse(Console.ReadLine().Trim(), out int option) && option > 0 && option <= 2)
            {
                return option;
            }
            else
            {
                InvalidOption();
            }
        }
    }

    public static int GetValidCountryMenuOption()
    {
        while (true)
        {
            Console.WriteLine("Enter an option:");
            if (int.TryParse(Console.ReadLine().Trim(), out int option) && option > 0 && option <= 3)
            {
                return option;
            }
            else
            {
                InvalidOption();
            }
        }
    }
}

// Methods for displaying a welcome screen and options on how to proceed
class Display
{

    public static void Greeting()
    {
        Console.WriteLine("Welcome To The Asset Tracker!");
    }

    public static void MainMenu()
    {
        Console.WriteLine("(1) View Existing Assets");
        Console.WriteLine("(2) Add New Asset");
        Console.WriteLine("(3) Quit");
    }




}
// Methods for adding assets
class AssetManager
{
    public static void AddAsset()
    {
        OfficeCountry countryDetails = OfficeCountry.SelectCountry();
        string country = countryDetails.Country;
        string currency = countryDetails.Currency;
        decimal exchangeRate = countryDetails.ExchangeRate;
        Console.WriteLine("Choose Asset Type:");
        Console.WriteLine("(1) Computer");
        Console.WriteLine("(2) Phone");

        int typeOption = UserInputValidation.GetValidTypeMenuOption();

        if (typeOption == 1)
        {
            Computer computer = new Computer(
                country,
                "Computer",
                UserInputValidation.GetValidBrand(),
                UserInputValidation.GetValidModel(),
                UserInputValidation.GetValidDate(),
                UserInputValidation.GetValidPrice(),
                currency,
                exchangeRate

                );
            Listmanager.AddElectronicAsset(computer);
        }
        else if (typeOption == 2)
        {
            Phone phone = new Phone(
                country,
                "Phone",
                UserInputValidation.GetValidBrand(),
                UserInputValidation.GetValidModel(),
                UserInputValidation.GetValidDate(),
                UserInputValidation.GetValidPrice(),
                currency,
                exchangeRate

                );
            Listmanager.AddElectronicAsset(phone);
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("New asset added!");
        Console.ResetColor();
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------------");
    }
}


