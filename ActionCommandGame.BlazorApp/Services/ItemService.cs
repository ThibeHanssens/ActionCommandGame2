using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.BlazorApp.Services
{
    public class ItemService
    {
        private readonly ItemSdk _sdk;

        public ItemService(ItemSdk sdk)
        {
            _sdk = sdk;
        }

        public async Task<ServiceResult<IList<ItemResult>>> GetAllAsync()
        {
            return await _sdk.Find();
        }

        public async Task<ServiceResult<ItemResult>> GetByIdAsync(int id)
        {
            return await _sdk.Get(id);
        }
    }
}
