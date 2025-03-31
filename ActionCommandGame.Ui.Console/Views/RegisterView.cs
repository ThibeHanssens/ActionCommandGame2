using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class RegisterView: IView
    {
        private readonly ITokenStore _tokenStore;
        private readonly IIdentityService<AuthenticationResult> _identityApi;
        private readonly NavigationManager _navigationManager;

        public RegisterView(
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
            await ConsoleBlockWriter.Write("Register");

            var token = await GetJwtToken();

            await _tokenStore.SaveTokenAsync(token);

            await _navigationManager.NavigateTo<PlayerSelectionView>();
        }

        private async Task<string?> GetJwtToken()
        {
            var request = await GetRegisterRequest();

            var result = await _identityApi.Register(request);

            if (!result.Success)
            {
                await ConsoleWriter.WriteText("Unable to register.");
                return await GetJwtToken();
            }

            return result.Token;
        }

        private async Task<UserRegistrationRequest> GetRegisterRequest()
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

            return new UserRegistrationRequest { Email = email, Password = password };
        }
    }
}
