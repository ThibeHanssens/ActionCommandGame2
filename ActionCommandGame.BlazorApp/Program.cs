using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using ActionCommandGame.BlazorApp;
using ActionCommandGame.BlazorApp.Settings;
using ActionCommandGame.BlazorApp.Stores;
using ActionCommandGame.BlazorApp.Security;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Sdk;
using ActionCommandGame.BlazorApp.Services;
using Blazored.LocalStorage;
using ActionCommandGame.Sdk.Abstractions;
using static ActionCommandGame.BlazorApp.Services.PlayerService;
using ActionCommandGame.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Ensure the appsettings.json file from wwwroot is added to the configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Ensure the GameSettings.cs file from ActionCommandGame.Settings is added to the configuration
builder.Services.AddSingleton(new GameSettings());

// Add root components: the App component and the HeadOutlet for <head> modifications.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure API settings from configuration (e.g. from appsettings.json).
var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);

// Register AppSettings in DI so that IOptions<AppSettings> gets the values from appsettings.json.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Check that the BaseUrl is configured.
if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
{
    throw new InvalidOperationException("API BaseUrl is not configured in appsettings.");
}

// Register the API/SDK services with the provided API BaseUrl.
// This extension method (AddApi) registers the necessary HTTP client and maps the SDK interfaces 
// (e.g. IGameService, IPlayerService, IItemService, IIdentityService, etc.) to their implementations.
builder.Services.AddApi(apiSettings.BaseUrl);

// Register all SDK implementations
builder.Services.AddScoped<IdentitySdk>();
builder.Services.AddScoped<GameSdk>();
builder.Services.AddScoped<GameEventSdk>();
builder.Services.AddScoped<ItemSdk>();
builder.Services.AddScoped<PlayerSdk>();
builder.Services.AddScoped<PlayerItemSdk>();
builder.Services.AddScoped<ProfileSdk>();

// Add Blazored LocalStorage for secure token storage.
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<MemoryStore>();

// Add authorization support and configure our custom authentication state provider.
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

// Register application-specific services.
builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<PlayerItemService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<AdminService>();

// Configure an HttpClient that automatically adds the JWT token to all API requests.
// This configuration is useful for any additional HTTP calls you might make outside the SDK.
//builder.Services.AddScoped(sp =>
//{
//    var client = new HttpClient { BaseAddress = new Uri(apiSettings.BaseUrl) };
//    // Retrieve the token from your token store.
//    //var token = sp.GetRequiredService<ITokenStore>().GetToken();
//    var token = sp.GetRequiredService<ITokenStore>().GetTokenAsync().GetAwaiter().GetResult();
//    Console.WriteLine($"API Base URL: {apiSettings.BaseUrl}");
//    if (!string.IsNullOrWhiteSpace(token))
//    {
//        Console.WriteLine($"Token added to Authorization header: {token}");
//        client.DefaultRequestHeaders.Authorization =
//            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//    }
//    return client;
//});

// register a named HttpClient for your API (no blocking, BaseAddress only)
builder.Services.AddHttpClient("ActionCommandGame", client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
});


// Build and run the application.
await builder.Build().RunAsync();
