using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class InventoryView: IView
    {
        private readonly MemoryStore _memoryStore;
        private readonly IPlayerItemService _playerItemApi;

        public InventoryView(
            MemoryStore memoryStore,
            IPlayerItemService playerItemApi)
        {
            _memoryStore = memoryStore;
            _playerItemApi = playerItemApi;
        }
        public async Task Show()
        {
            await ConsoleBlockWriter.Write("Inventory");

            var filter = new PlayerItemFilter { PlayerId = _memoryStore.CurrentPlayerId };
            var inventoryResult = await _playerItemApi.Find(filter);

            if (inventoryResult.Data is null || !inventoryResult.Data.Any())
            {
                await ConsoleWriter.WriteText("Your inventory is empty.", ConsoleColor.DarkGray);
                return;
            }

            foreach (var playerItem in inventoryResult.Data)
            {
                await ShowPlayerItem(playerItem);
            }
        }

        private static async Task ShowPlayerItem(PlayerItemResult playerItem)
        {
            await ConsoleWriter.WriteText($"\t{playerItem.Name}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(playerItem.Description))
            {
                await ConsoleWriter.WriteText($"\t\t{playerItem.Description}");
            }
            if (playerItem.Fuel > 0)
            {
                await ConsoleWriter.WriteText($"\t\tFuel: {playerItem.RemainingFuel}/{playerItem.Fuel}");
            }
            if (playerItem.Attack > 0)
            {
                await ConsoleWriter.WriteText($"\t\tAttack: {playerItem.RemainingAttack}/{playerItem.Attack}");
            }
            if (playerItem.Defense > 0)
            {
                await ConsoleWriter.WriteText($"\t\tDefense: {playerItem.RemainingDefense}/{playerItem.Defense}");
            }
            if (playerItem.ActionCooldownSeconds > 0)
            {
                await ConsoleWriter.WriteText($"\t\tCooldown seconds: {playerItem.ActionCooldownSeconds}");
            }
        }
    }
}
