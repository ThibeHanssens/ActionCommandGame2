using ActionCommandGame.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class LeaderboardView: IView
    {
        private readonly MemoryStore _memoryStore;
        private readonly IPlayerService _playerSdk;

        public LeaderboardView(
            MemoryStore memoryStore,
            IPlayerService playerSdk)
        {
            _memoryStore = memoryStore;
            _playerSdk = playerSdk;
        }

        public async Task Show()
        {
            await ConsoleBlockWriter.Write("Leaderboard");

            var playersResult = await _playerSdk.Find(new PlayerFilter());

            if (playersResult.Data is null || !playersResult.Data.Any())
            {
                await ConsoleWriter.WriteText("There are no players on the leaderboard", ConsoleColor.DarkGray);
                return;
            }

            var orderedPlayers = playersResult.Data.OrderByDescending(p => p.Experience).ToList();

            foreach (var player in orderedPlayers)
            {
                var text = $"\tLevel {player.GetLevel()} {player.Name} ({player.Experience})";
                if (player.Id == _memoryStore.CurrentPlayerId)
                {
                    await ConsoleWriter.WriteText(text, ConsoleColor.Yellow);
                }
                else
                {
                    await ConsoleWriter.WriteText(text);
                }
            }
        }
    }
}
