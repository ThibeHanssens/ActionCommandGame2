using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Extensions.Filters;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerItemService : IPlayerItemService
    {
        private readonly ActionCommandGameDbContext _dbContext;

        public PlayerItemService(ActionCommandGameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<PlayerItemResult>> Get(int id)
        {
            var playerItem = await _dbContext.PlayerItems
                .ProjectToResult()
                .SingleOrDefaultAsync(pi => pi.Id == id);

            return new ServiceResult<PlayerItemResult>(playerItem);
        }

        public async Task<ServiceResult<IList<PlayerItemResult>>> Find(PlayerItemFilter filter)
        {
            var playerItems = await _dbContext.PlayerItems
                .ApplyFilter(filter)
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<PlayerItemResult>>(playerItems);
        }

        public async Task<ServiceResult<PlayerItemResult>> Create(int playerId, int itemId)
        {
            var player = _dbContext.Players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<PlayerItemResult>().PlayerNotFound();
            }

            var item = _dbContext.Items.SingleOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<PlayerItemResult>().ItemNotFound();
            }

            var playerItem = new PlayerItem
            {
                ItemId = itemId,
                Item = item,
                PlayerId = playerId,
                Player = player
            };
            _dbContext.PlayerItems.Add(playerItem);
            player.Inventory.Add(playerItem);
            item.PlayerItems.Add(playerItem);

            //Auto Equip the item you bought
            if (item.Fuel > 0)
            {
                playerItem.RemainingFuel = item.Fuel;
                player.CurrentFuelPlayerItemId = playerItem.Id;
                player.CurrentFuelPlayerItem = playerItem;
            }
            if (item.Attack > 0)
            {
                playerItem.RemainingAttack = item.Attack;
                player.CurrentAttackPlayerItemId = playerItem.Id;
                player.CurrentAttackPlayerItem = playerItem;
            }
            if (item.Defense > 0)
            {
                playerItem.RemainingDefense = item.Defense;
                player.CurrentDefensePlayerItemId = playerItem.Id;
                player.CurrentDefensePlayerItem = playerItem;
            }

            await _dbContext.SaveChangesAsync();

            return await Get(playerItem.Id);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var playerItem = await _dbContext.PlayerItems
                .Include(pi => pi.Player)
                .Include(pi => pi.Item)
                .SingleOrDefaultAsync(pi => pi.Id == id);

            if (playerItem == null)
            {
                return new ServiceResult().NotFound();
            }
            
            var player = playerItem.Player;
            player.Inventory.Remove(playerItem);
            
            var item = playerItem.Item;
            item.PlayerItems.Remove(playerItem);

            //Clear up equipment
            if (player.CurrentFuelPlayerItemId == id)
            {
                player.CurrentFuelPlayerItemId = null;
            }
            if (player.CurrentAttackPlayerItemId == id)
            {
                player.CurrentAttackPlayerItemId = null;
            }
            if (player.CurrentDefensePlayerItemId == id)
            {
                player.CurrentDefensePlayerItemId = null;
            }

            _dbContext.PlayerItems.Remove(playerItem);

            //Save Changes
            await _dbContext.SaveChangesAsync();

            return new ServiceResult();
        }
        
    }
}
