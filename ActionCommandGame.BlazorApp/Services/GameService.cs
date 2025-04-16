using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.BlazorApp.Services
{
    public class GameService
    {
        private readonly GameSdk _sdk;

        public GameService(GameSdk sdk)
        {
            _sdk = sdk;
        }

        public async Task<ServiceResult<GameResult>> PerformActionAsync(int playerId)
        {
            return await _sdk.PerformAction(playerId);
        }

        public async Task<ServiceResult<BuyResult>> BuyAsync(int playerId, int itemId)
        {
            return await _sdk.Buy(playerId, itemId);
        }
    }
}
