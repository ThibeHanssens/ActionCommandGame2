using ActionCommandGame.Abstractions;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Extensions.Filters;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly ActionCommandGameDbContext _database;
        private readonly IUserContext<string?> _userContext;

        public PlayerService(
            ActionCommandGameDbContext database,
            IUserContext<string?> userContext)
        {
            _database = database;
            _userContext = userContext;
        }

        public async Task<ServiceResult<PlayerResult>> Get(int id)
        {
            var player = await _database.Players
                .ProjectToResult()
                .SingleOrDefaultAsync(p => p.Id == id);

            return new ServiceResult<PlayerResult>(player);
        }

        public async Task<ServiceResult<IList<PlayerResult>>> Find(PlayerFilter filter)
        {
            var authenticatedUserId = _userContext.UserId;

            var players = await _database.Players
                .ApplyFilter(filter, authenticatedUserId)
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<PlayerResult>>(players);
        }

        public async Task<ServiceResult<PlayerResult>> Create(PlayerCreateRequest request)
        {
            var userId = _userContext.UserId!;
            var player = new Player
            {
                UserId = userId,
                Name = request.Name,
                Money = 100,
                Experience = 0
            };
            _database.Players.Add(player);
            await _database.SaveChangesAsync();
            var dto = new PlayerResult
            {
                Id = player.Id,
                Name = player.Name,
                UserId = player.UserId,
                Money = player.Money,
                Experience = player.Experience
            };
            return new ServiceResult<PlayerResult>(dto);
        }
    }
}
