using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        Task<ServiceResult<PositiveGameEventResult>> GetRandomPositiveGameEvent(bool hasAttackItem);
        Task<ServiceResult<IList<PositiveGameEventResult>>> Find();
        Task<ServiceResult<PositiveGameEventResult>> Create(PositiveGameEventCreateRequest req);
        Task<ServiceResult> Delete(int id);
    }
}
