using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface INegativeGameEventService
    {
        Task<ServiceResult<NegativeGameEventResult>> GetRandomNegativeGameEvent();
        Task<ServiceResult<IList<NegativeGameEventResult>>> Find();
        Task<ServiceResult<NegativeGameEventResult>> Create(NegativeGameEventCreateRequest req);
        Task<ServiceResult> Delete(int id);
    }
}
