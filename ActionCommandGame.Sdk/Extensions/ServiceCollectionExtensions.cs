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

        services.AddTransient<IGameService, GameSdk>();
        services.AddTransient<IPlayerService, PlayerSdk>();
        services.AddTransient<IItemService, ItemSdk>();
        services.AddTransient<IPlayerItemService, PlayerItemSdk>();
        services.AddTransient<IIdentityService<AuthenticationResult>, IdentitySdk>();
    }
}