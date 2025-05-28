using ComplexTask_LINQ.Database;
using ComplexTask_LINQ.DbModels;
using ComplexTask_LINQ.Models;
using ComplexTask_LINQ.Provider.CustomProvider;
using ComplexTask_LINQ.Services;
using ComplexTask_LINQ.Test;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        var searchService = serviceProvider.GetRequiredService<SearchService>();

        var books = searchService.SearchBooks(10.0, 50.0);
        Console.WriteLine($"Books found: {books.Count}");
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IPgDatabaseConnection>(_ =>
            new PgDatabaseConnection("YourConnectionStringHere"));

        services.AddSingleton<ICustomQueryProvider, CustomQueryProvider>();

        services.AddTransient<SearchService>();
    }
}
//Console.WriteLine("Hello, World!");


//var connectionString = "Host=localhost;Port=5432;Database=mydb;Username=myuser;Password=mypassword";
//var databaseConnection = new PgDatabaseConnection(connectionString);
//var provider = new CustomQueryProvider(databaseConnection);

//var books = new CustomQueryable<Book>(provider);

//// Пример LINQ-запроса
//var query = books.Where(book => book.Author == "John");

//foreach (var book in query)
//{
//    Console.WriteLine($"Title: {book.Name}, Author: {book.Author}");
//}