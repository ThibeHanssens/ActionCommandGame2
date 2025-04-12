using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.BlazorApp.Services
{
    public class PlayerItemService
    {
        private readonly PlayerItemApi _sdk;

        public PlayerItemService(PlayerItemApi sdk)
        {
            _sdk = sdk;
        }

        // Retrieves a list of player items using the provided filter.
        public async Task<ServiceResult<IList<PlayerItemResult>>> GetAllAsync(PlayerItemFilter filter)
        {
            return await _sdk.FindAsync(filter);
        }

        // Retrieves a single player item by ID.
        public Task<ServiceResult<PlayerItemResult>> GetByIdAsync(int id)
        {
            throw new NotImplementedException("GetById is not implemented in the SDK yet.");
        }

        // Creates a new player item for the specified player and item.
        public Task<ServiceResult<PlayerItemResult>> CreateAsync(int playerId, int itemId)
        {
            throw new NotImplementedException("Create is not implemented in the SDK yet.");
        }

        // Deletes a player item by ID.
        public Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException("Delete is not implemented in the SDK yet.");
        }
    }
}
