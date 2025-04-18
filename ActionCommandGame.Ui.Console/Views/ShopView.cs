using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class ShopView: IView
    {
        private readonly IItemService _itemSdk;

        public ShopView(IItemService itemSdk)
        {
            _itemSdk = itemSdk;
        }
        public async Task Show()
        {
            await ConsoleBlockWriter.Write("Shop");
            var shopItems = await _itemSdk.Find();

            if (shopItems.Data is null)
            {
                await ConsoleWriter.WriteText("There are no available Shop items", ConsoleColor.DarkGray);
                return;
            }

            await ConsoleWriter.WriteText("Available Shop Items", ConsoleColor.Green);
            foreach (var item in shopItems.Data)
            {
                await ShowItem(item);
            }

            await ConsoleWriter.WriteLine();
        }

        private static async Task ShowItem(ItemResult item)
        {
            await ConsoleWriter.WriteText($"\t[{item.Id}] {item.Name} €{item.Price}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(item.Description))
            {
                await ConsoleWriter.WriteText($"\t\t{item.Description}");
            }
            if (item.Fuel > 0)
            {
                await ConsoleWriter.WriteText("\t\tFuel: ", ConsoleColor.White, false);
                await ConsoleWriter.WriteText($"{item.Fuel}");
            }
            if (item.Attack > 0)
            {
                await ConsoleWriter.WriteText("\t\tAttack: ", ConsoleColor.White, false);
                await ConsoleWriter.WriteText($"{item.Attack}");
            }
            if (item.Defense > 0)
            {
                await ConsoleWriter.WriteText("\t\tDefense: ", ConsoleColor.White, false);
                await ConsoleWriter.WriteText($"{item.Defense}");
            }
            if (item.ActionCooldownSeconds > 0)
            {
                await ConsoleWriter.WriteText("\t\tCooldown seconds: ", ConsoleColor.White, false);
                await ConsoleWriter.WriteText($"{item.ActionCooldownSeconds}");
            }
        }
    }
}
