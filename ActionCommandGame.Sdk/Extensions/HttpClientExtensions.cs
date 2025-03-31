using System.Net.Http.Headers;

namespace ActionCommandGame.Sdk.Extensions
{
    internal static class HttpClientExtensions
    {
        public static void AddAuthorization(this HttpClient httpClient, string? bearerToken)
        {
            if (bearerToken is null)
            {
                return;
            }
            httpClient.DefaultRequestHeaders.AddAuthorization(bearerToken);
        }

        public static void AddAuthorization(this HttpRequestHeaders httpRequestHeaders, string? bearerToken)
        {
            if (bearerToken is null)
            {
                return;
            }
            if (httpRequestHeaders.Contains("Authorization"))
            {
                httpRequestHeaders.Remove("Authorization");
            }
            httpRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
        }
	}
}
