using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActionCommandGame.BlazorApp.Services
{
    public class AdminService
    {
        private readonly ItemSdk _itemSdk;
        private readonly GameEventSdk _geSdk;

        public AdminService(ItemSdk itemSdk, GameEventSdk geSdk)
        {
            _itemSdk = itemSdk;
            _geSdk = geSdk;
        }

        // Items
        public Task<ServiceResult<IList<ItemResult>>> GetAllItemsAsync()
            => _itemSdk.Find();
        public Task<ServiceResult<ItemResult>> CreateItemAsync(ItemResult dto)
            => _itemSdk.Create(dto);
        public Task<ServiceResult<ItemResult>> UpdateItemAsync(ItemResult dto)
            => _itemSdk.Update(dto);
        public Task<ServiceResult> DeleteItemAsync(int id)
            => _itemSdk.Delete(id);

        // GameEvents
        public Task<ServiceResult<IList<PositiveGameEventResult>>> GetAllPositiveAsync()
            => _geSdk.FindPositive();
        public Task<ServiceResult<PositiveGameEventResult>> CreatePositiveAsync(PositiveGameEventResult dto)
            => _geSdk.CreatePositive(dto);
        public Task<ServiceResult> DeletePositiveAsync(int id)
            => _geSdk.DeletePositive(id);

        public Task<ServiceResult<IList<NegativeGameEventResult>>> GetAllNegativeAsync()
            => _geSdk.FindNegative();
        public Task<ServiceResult<NegativeGameEventResult>> CreateNegativeAsync(NegativeGameEventResult dto)
            => _geSdk.CreateNegative(dto);
        public Task<ServiceResult> DeleteNegativeAsync(int id)
            => _geSdk.DeleteNegative(id);
    }
}
