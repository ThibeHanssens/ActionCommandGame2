using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, string? baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return;
        }

        services.AddApi(new Uri(baseUrl));
    }

    public static void AddApi(this IServiceCollection services, Uri baseUri)
    {
        services.AddHttpClient("ActionCommandGame", (_, c) =>
            {
                c.BaseAddress = baseUri;
            });

        services.AddTransient<IGameService, GameApi>();
        services.AddTransient<IPlayerService, PlayerApi>();
        services.AddTransient<IItemService, ItemApi>();
        services.AddTransient<IPlayerItemService, PlayerItemApi>();
        services.AddTransient<IIdentityService<AuthenticationResult>, IdentityApi>();
    }
}