using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IItemService
    {
        Task<ServiceResult<IList<ItemResult>>> Find();
        Task<ServiceResult<ItemResult>> Get(int id);
        Task<ServiceResult<ItemResult>> Create(ItemCreateRequest request);
        Task<ServiceResult<ItemResult>> Update(ItemUpdateRequest request);
        Task<ServiceResult> Delete(int id);
    }
}
