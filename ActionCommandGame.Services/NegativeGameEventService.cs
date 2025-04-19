using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class NegativeGameEventService: INegativeGameEventService
    {
        private readonly ActionCommandGameDbContext _database;

        public NegativeGameEventService(ActionCommandGameDbContext database)
        {
            _database = database;
        }
        
        public async Task<ServiceResult<NegativeGameEventResult>> GetRandomNegativeGameEvent()
        {
            var gameEvents = await Find();
            var randomEvent = GameEventHelper.GetRandomNegativeGameEvent(gameEvents.Data);
            return new ServiceResult<NegativeGameEventResult>(randomEvent);
        }

        public async Task<ServiceResult<IList<NegativeGameEventResult>>> Find()
        {
            var negativeGameEvents = await _database.NegativeGameEvents
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<NegativeGameEventResult>>(negativeGameEvents);
        }

        public async Task<ServiceResult<NegativeGameEventResult>> Create(NegativeGameEventCreateRequest req)
        {
            var e = new NegativeGameEvent
            {
                Name = req.Name,
                Description = req.Description,
                DefenseWithGearDescription = req.DefenseWithGearDescription,
                DefenseWithoutGearDescription = req.DefenseWithoutGearDescription,
                DefenseLoss = req.DefenseLoss,
                Probability = req.Probability
            };
            _database.NegativeGameEvents.Add(e);
            await _database.SaveChangesAsync();

            var dto = await _database.NegativeGameEvents
                .Where(x => x.Id == e.Id)
                .ProjectToResult()
                .SingleAsync();

            return new ServiceResult<NegativeGameEventResult>(dto);
        }
        public async Task<ServiceResult> Delete(int id)
        {
            var e = await _database.NegativeGameEvents.FindAsync(id);
            if (e == null) return new ServiceResult().NotFound();
            _database.NegativeGameEvents.Remove(e);
            await _database.SaveChangesAsync();
            return new ServiceResult();
        }
    }
}
