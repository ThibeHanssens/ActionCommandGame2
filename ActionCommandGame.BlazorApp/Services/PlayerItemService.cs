using System.Threading.Tasks;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.BlazorApp.Services
{
    public class PlayerItemService
    {
        private readonly PlayerItemSdk _sdk;

        public PlayerItemService(PlayerItemSdk sdk)
        {
            _sdk = sdk;
        }

        // Retrieves a list of player items using the provided filter.
        public async Task<ServiceResult<IList<PlayerItemResult>>> GetAllAsync(PlayerItemFilter filter)
            => await _sdk.FindAsync(filter);

        // Retrieves a single player item by ID.
        public async Task<ServiceResult<PlayerItemResult>> GetByIdAsync(int id)
            => await _sdk.Get(id);

        // Creates a new player item for the specified player and item.
        public async Task<ServiceResult<PlayerItemResult>> CreateAsync(int playerId, int itemId)
            => await _sdk.Create(playerId, itemId);

        // Deletes a player item by ID.
        public async Task<ServiceResult> DeleteAsync(int id)
            => await _sdk.Delete(id);
    }
}
