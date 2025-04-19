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
    public class PositiveGameEventService: IPositiveGameEventService
    {
        private readonly ActionCommandGameDbContext _database;

        public PositiveGameEventService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PositiveGameEventResult>> GetRandomPositiveGameEvent(bool hasAttackItem)
        {
            var query = _database.PositiveGameEvents.AsQueryable();

            //If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Money < 50);
            }

            var gameEvents = await query
                .ProjectToResult()
                .ToListAsync();

            var randomEvent = GameEventHelper.GetRandomPositiveGameEvent(gameEvents);

            return new ServiceResult<PositiveGameEventResult>(randomEvent);
        }

        public async Task<ServiceResult<IList<PositiveGameEventResult>>> Find()
        {
            var all = await _database.PositiveGameEvents
                .ProjectToResult()
                .ToListAsync();
            return new ServiceResult<IList<PositiveGameEventResult>>(all);
        }

        public async Task<ServiceResult<PositiveGameEventResult>> Create(PositiveGameEventCreateRequest req)
        {
            var e = new PositiveGameEvent
            {
                Name = req.Name,
                Description = req.Description,
                Money = req.Money,
                Experience = req.Experience,
                Probability = req.Probability
            };
            _database.PositiveGameEvents.Add(e);
            await _database.SaveChangesAsync();

            var dto = await _database.PositiveGameEvents
                .Where(x => x.Id == e.Id)
                .ProjectToResult()
                .SingleAsync();
            return new ServiceResult<PositiveGameEventResult>(dto);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var e = await _database.PositiveGameEvents.FindAsync(id);
            if (e == null) return new ServiceResult().NotFound();
            _database.PositiveGameEvents.Remove(e);
            await _database.SaveChangesAsync();
            return new ServiceResult();
        }
    }
}
