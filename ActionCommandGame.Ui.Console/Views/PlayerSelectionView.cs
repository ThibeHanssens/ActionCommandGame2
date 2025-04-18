using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class PlayerSelectionView: IView
    {
        private readonly MemoryStore _memoryStore;
        private readonly IPlayerService _playerSdk;
        private readonly NavigationManager _navigationManager;

        public PlayerSelectionView(
            MemoryStore memoryStore,
            IPlayerService playerSdk,
            NavigationManager navigationManager)
        {
            _memoryStore = memoryStore;
            _playerSdk = playerSdk;
            _navigationManager = navigationManager;
        }

        public async Task Show()
        {
            var playerId = await ReadPlayerId();

            if (!playerId.HasValue)
            {
                await ConsoleBlockWriter.Write("Could not load players.", 1, ConsoleColor.Red);
                await _navigationManager.NavigateTo<ExitView>(false);
                return;
            }

            _memoryStore.CurrentPlayerId = playerId.Value;

           
            await _navigationManager.NavigateTo<GameView>();
        }

        private async Task<int?> ReadPlayerId()
        {
            var filter = new PlayerFilter { FilterUserPlayers = true };
            var players = await _playerSdk.Find(filter);

            if (players.Data is null || !players.Data.Any())
            {
                await ConsoleWriter.WriteText("No players found");
                return null;
            }

            var playerTextLines = players.Data.Select(p => $"{p.Id}. {p.Name}").ToList();
            playerTextLines.Add("");
            playerTextLines.Add("Choose your Player by typing the correct number.");
            await ConsoleBlockWriter.Write(playerTextLines, 1, ConsoleColor.White);
            await ConsoleWriter.WriteText("Your player: ", ConsoleColor.White, false);
            var playerInput = Console.ReadLine();

            if (!int.TryParse(playerInput, out int playerId))
            {
                await ConsoleWriter.Clear();

                await ConsoleWriter.WriteText("Please enter a correct player number.", ConsoleColor.Blue);

                return await ReadPlayerId();
            }

            return playerId;
        }
    }
}
