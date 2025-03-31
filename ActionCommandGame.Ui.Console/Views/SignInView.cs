using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class SignInView: IView
    {
        private readonly ITokenStore _tokenStore;
        private readonly IIdentityService<AuthenticationResult> _identityApi;
        private readonly NavigationManager _navigationManager;

        public SignInView(
            ITokenStore tokenStore,
            IIdentityService<AuthenticationResult> identityApi,
            NavigationManager navigationManager)
        {
            _tokenStore = tokenStore;
            _identityApi = identityApi;
            _navigationManager = navigationManager;
        }
        public async Task Show()
        {
            ConsoleWriter.WriteTitle("Sign In");

            var token = await GetJwtToken();

            await _tokenStore.SaveTokenAsync(token);

            await _navigationManager.NavigateTo<PlayerSelectionView>();
        }

        private async Task<string?> GetJwtToken()
        {
            var request = await GetSignInRequest();

            var result = await _identityApi.SignIn(request);

            if (!result.Success)
            {
                Console.Clear();
                await ConsoleWriter.WriteText("Email/Password combination is wrong.");
                return await GetJwtToken();
            }

            return result.Token;
        }

        private async Task<UserSignInRequest> GetSignInRequest()
        {
            await ConsoleWriter.WriteText("Please type your email: ", ConsoleColor.White, false);
            var email = Console.ReadLine() ?? string.Empty;

            await ConsoleWriter.WriteText("Please type your password: ", ConsoleColor.White, false);
            string password = string.Empty;

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
                Console.Write("*");
            }

            return new UserSignInRequest { Email = email, Password = password };
        }
    }
}
