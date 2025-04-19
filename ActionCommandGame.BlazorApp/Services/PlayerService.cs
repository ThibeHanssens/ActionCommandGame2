using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.BlazorApp.Services
{
    public class PlayerService
    {
        private readonly PlayerSdk _sdk;

        public PlayerService(PlayerSdk sdk)
        {
            _sdk = sdk;
        }

        // Retrieves a specific player by ID via the SDK.
        public async Task<ServiceResult<PlayerResult>> GetByIdAsync(int id)
        {
            return await _sdk.Get(id);
        }


        // Retrieves a list of players matching the provided filter.
        public async Task<ServiceResult<IList<PlayerResult>>> GetAllAsync(PlayerFilter filter)
        {
            return await _sdk.Find(filter);
        }

        // Create a new player
        public async Task<ServiceResult<PlayerResult>> CreateAsync(PlayerCreateRequest request)
        {
            return await _sdk.Create(request);
        }
    }
}
