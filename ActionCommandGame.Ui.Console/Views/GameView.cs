using ActionCommandGame.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Settings;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class GameView : IView
    {
        private readonly AppSettings _settings;
        private readonly MemoryStore _memoryStore;
        private readonly NavigationManager _navigationManager;
        private readonly IGameService _gameApi;
        private readonly IPlayerService _playerApi;

        public GameView(
            AppSettings settings,
            MemoryStore memoryStore,
            NavigationManager navigationManager,
            IGameService gameApi,
            IPlayerService playerApi)
        {
            _settings = settings;
            _memoryStore = memoryStore;
            _navigationManager = navigationManager;
            _gameApi = gameApi;
            _playerApi = playerApi;
        }

        public async Task Show()
        {
            await ConsoleWriter.WriteText($"Play your game. Try typing \"help\" or \"{_settings.ActionCommand}\"", ConsoleColor.Yellow);

            //Get the player from somewhere
            var currentPlayerId = _memoryStore.CurrentPlayerId;

            while (true)
            {

                await ConsoleWriter.WriteText($"{_settings.CommandPromptText} ", ConsoleColor.DarkGray, false);

                string? command = Console.ReadLine();

                await ConsoleWriter.Clear();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                if (CheckCommand(command, new[] { "change player", "player", "change" }))
                {
                    await _navigationManager.NavigateTo<PlayerSelectionView>();
                    break;
                }

                if (CheckCommand(command, new[] { "exit", "quit", "stop" }))
                {
                    await _navigationManager.NavigateTo<ExitView>();
                    break;
                }

                if (!string.IsNullOrWhiteSpace(_settings.ActionCommand) && CheckCommand(command, [_settings.ActionCommand]))
                {
                    await PerformAction(currentPlayerId);
                    await ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "shop", "store" }))
                {
                    await _navigationManager.NavigateTo<ShopView>();
                }

                if (CheckCommand(command, new[] { "buy", "purchase", "get" }))
                {
                    var itemId = GetIdParameterFromCommand(command);

                    if (!itemId.HasValue)
                    {
                        await ConsoleWriter.WriteText("I have no idea what you mean. I have tagged every item with a number. Please give me that number.", ConsoleColor.Red);
                        continue;
                    }

                    await Buy(currentPlayerId, itemId.Value);
                }

                if (CheckCommand(command, new[] { "bal", "balance", "money", "xp", "level", "statistics", "stats", "stat", "info" }))
                {
                    await ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "leaderboard", "lead", "top", "rank", "ranking" }))
                {
                    await _navigationManager.NavigateTo<LeaderboardView>();
                }

                if (CheckCommand(command, new[] { "inventory", "inv", "bag", "backpack" }))
                {
                    await _navigationManager.NavigateTo<InventoryView>();
                }

                if (CheckCommand(command, new[] { "?", "help", "h", "commands" }))
                {
                    await _navigationManager.NavigateTo<HelpView>();
                }
            }
        }



        private static bool CheckCommand(string command, IList<string> matchingCommands)
        {
            return matchingCommands.Any(c => command.ToLower().StartsWith(c.ToLower()));
        }

        private async Task ShowStats(int playerId)
        {
            var playerResult = await _playerApi.Get(playerId);

            if (!playerResult.IsSuccess || playerResult.Data is null)
            {
                return;
            }

            var player = playerResult.Data;

            //Check food consumption
            if (player.CurrentFuelId != null)
            {
                await ConsoleWriter.WriteText($"[{player.CurrentFuelName}] ", ConsoleColor.Yellow, false);
                await ConsoleWriter.WriteText($"{player.RemainingFuel}/{player.TotalFuel}  ", null, false);
            }
            else
            {
                await ConsoleWriter.WriteText("[Food] ", ConsoleColor.Red, false);
                await ConsoleWriter.WriteText("nothing ", null, false);
            }

            //Check attack consumption
            if (player.CurrentAttackId != null)
            {
                await ConsoleWriter.WriteText($"[{player.CurrentAttackName}] ", ConsoleColor.Yellow, false);
                await ConsoleWriter.WriteText($"{player.RemainingAttack}/{player.TotalAttack}  ", null, false);
            }
            else
            {
                await ConsoleWriter.WriteText("[Attack] ", ConsoleColor.Red, false);
                await ConsoleWriter.WriteText("nothing ", null, false);
            }

            //Check defense consumption
            if (player.CurrentDefenseId != null)
            {
                await ConsoleWriter.WriteText($"[{player.CurrentDefenseName}] ", ConsoleColor.Yellow, false);
                await ConsoleWriter.WriteText($"{player.RemainingDefense}/{player.TotalDefense}  ", null, false);
            }
            else
            {
                await ConsoleWriter.WriteText("[Defense] ", ConsoleColor.Red, false);
                await ConsoleWriter.WriteText("nothing ", null, false);
            }

            await ConsoleWriter.WriteText("[Money] ", ConsoleColor.Yellow, false);
            await ConsoleWriter.WriteText($"€{player.Money}  ", null, false);
            await ConsoleWriter.WriteText("[Level] ", ConsoleColor.Yellow, false);
            await ConsoleWriter.WriteText($"{player.GetLevel()} ({player.Experience}/{player.GetExperienceForNextLevel()})  ", null, false);

            await ConsoleWriter.WriteLine();
            await ConsoleWriter.WriteLine();
        }

        private async Task PerformAction(int playerId)
        {
            var result = await _gameApi.PerformAction(playerId);

            if (!result.IsSuccess || result.Data is null)
            {
                await ConsoleWriter.WriteText("Could not perform action.");
                return;
            }

            var player = result.Data.Player;
            var positiveGameEvent = result.Data.PositiveGameEvent;
            var negativeGameEvent = result.Data.NegativeGameEvent;

            if (player is null)
            {
                await _navigationManager.NavigateTo<PlayerSelectionView>();
                await ConsoleWriter.WriteText("Player was not found. Please select a player.", ConsoleColor.Red);
                return;
            }

            if (positiveGameEvent != null)
            {
                if (string.IsNullOrWhiteSpace(_settings.ActionText))
                {
                    _settings.ActionText = "[ACTION]";
                }
                await ConsoleWriter.WriteText($"{string.Format(_settings.ActionText, player.Name)} ",
                    ConsoleColor.Green, false);
                await ConsoleWriter.WriteText(positiveGameEvent.Name, ConsoleColor.White);
                if (!string.IsNullOrWhiteSpace(positiveGameEvent.Description))
                {
                    await ConsoleWriter.WriteText(positiveGameEvent.Description);
                }
                if (positiveGameEvent.Money > 0)
                {
                    await ConsoleWriter.WriteText($"€{positiveGameEvent.Money}", ConsoleColor.Yellow, false);
                    await ConsoleWriter.WriteText(" has been added to your account.");
                }
            }

            if (negativeGameEvent != null)
            {
                await ConsoleWriter.WriteText(negativeGameEvent.Name, ConsoleColor.Blue);
                if (!string.IsNullOrWhiteSpace(negativeGameEvent.Description))
                {
                    await ConsoleWriter.WriteText(negativeGameEvent.Description);
                }
                await ConsoleWriter.WriteMessages(result.Data.NegativeGameEventMessages);
            }

            await ConsoleWriter.WriteMessages(result.Messages);

            await ConsoleWriter.WriteLine();
        }

        private async Task Buy(int playerId, int itemId)
        {
            var result = await _gameApi.Buy(playerId, itemId);

            if (!result.IsSuccess || result.Data is null)
            {
                var errorMessages = result.Messages.Where(m => m.MessagePriority == MessagePriority.Error)
                    .ToList();
                await ConsoleWriter.WriteMessages(errorMessages);
                return;
            }

            await ConsoleWriter.WriteText($"You bought {result.Data.Item.Name} for €{result.Data.Item.Price}");
            await ConsoleWriter.WriteText($"Thank you for shopping. Your current balance is €{result.Data.Player.Money}.");

            //Check if there are info and warning messages
            var nonErrorMessages =
                result.Messages.Where(m => m.MessagePriority == MessagePriority.Error).ToList();
            await ConsoleWriter.WriteMessages(nonErrorMessages);


            Console.WriteLine();
        }

        private int? GetIdParameterFromCommand(string command)
        {
            var commandParts = command.Split(" ");
            if (commandParts.Length == 1)
            {
                return null;
            }
            var idPart = commandParts[1];

            int.TryParse(idPart, out var itemId);

            return itemId;
        }
    }
}
