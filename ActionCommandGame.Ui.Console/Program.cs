using System.Text;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Settings;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using ActionCommandGame.Ui.ConsoleApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Build configuration
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();

// Set up Dependency Injection
var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection, configuration);
var serviceProvider = serviceCollection.BuildServiceProvider();

// Retrieve the NavigationManager
var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();

// Set console output to UTF8
Console.OutputEncoding = Encoding.UTF8;

// Run the application
await navigationManager.NavigateTo<TitleView>();
// Method to configure services and register dependencies
static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Bind AppSettings section of configuration to our AppSettings object
    var appSettings = new AppSettings();
    configuration.Bind(nameof(AppSettings), appSettings);
    services.AddSingleton(appSettings);

    // Register stores
    services.AddSingleton<MemoryStore>();
    services.AddSingleton<ITokenStore, TokenStore>();

    // Register navigation
    services.AddTransient<NavigationManager>();

    // Register views
    services.AddTransient<ExitView>();
    services.AddTransient<GameView>();
    services.AddTransient<HelpView>();
    services.AddTransient<InventoryView>();
    services.AddTransient<LeaderboardView>();
    services.AddTransient<PlayerSelectionView>();
    services.AddTransient<RegisterView>();
    services.AddTransient<ShopView>();
    services.AddTransient<SignInView>();
    services.AddTransient<TitleView>();

    // Register SDK API classes (using a custom extension method, adjust namespace as needed)
    services.AddApi(appSettings.ApiBaseUrl);
}
