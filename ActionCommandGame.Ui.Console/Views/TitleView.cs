using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Settings;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class TitleView: IView
    {
        private readonly AppSettings _appSettings;
        private readonly NavigationManager _navigationManager;

        private IList<string> _messages = new List<string>();

        public TitleView(
            AppSettings appSettings,
            NavigationManager navigationManager)
        {
            _appSettings = appSettings;
            _navigationManager = navigationManager;
        }

        public async Task Show()
        {
            Console.Clear();
            if (string.IsNullOrWhiteSpace(_appSettings.GameName))
            {
                _appSettings.GameName = "ActionCommandGame";
            }

            var titleLines = new List<string>
            {
                "Vives Development Studios",
                "presents",
                "",
                _appSettings.GameName,
                "",
                "\"An amazing adventure - 87%\" - any gaming magazine"
            };

            await ConsoleBlockWriter.Write(titleLines, 4, ConsoleColor.Blue);

            await ConsoleWriter.WriteText("Type \"signin\" to sign in with an existing account or \"register\" to register a new account.");
            await ConsoleWriter.WriteText("Press \"exit\" to quit.", ConsoleColor.DarkGray);

            if (_messages.Any())
            {
                foreach (var message in _messages)
                {
                    await ConsoleWriter.WriteText(message);
                }
            }

            _messages.Clear();

            var command = Console.ReadLine();

            switch (command)
            {
                case "signin":
                    await _navigationManager.NavigateTo<SignInView>();
                    break;
                case "register":
                    await _navigationManager.NavigateTo<RegisterView>();
                    break;
                case "exit":
                    await _navigationManager.NavigateTo<ExitView>();
                    break;
                default:
                    _messages.Add("Wrong command. Please try again.");
                    await Show();
                    break;
            }
        }
    }
}
