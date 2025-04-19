using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class ItemService: IItemService
    {
        private readonly ActionCommandGameDbContext _dbContext;

        public ItemService(ActionCommandGameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<IList<ItemResult>>> Find()
        {
            var items = await _dbContext.Items
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<ItemResult>>(items);
        }

        public async Task<ServiceResult<ItemResult>> Get(int id)
        {
            var item = await _dbContext.Items
                .ProjectToResult()
                .FirstOrDefaultAsync(i => i.Id == id);

            return new ServiceResult<ItemResult>(item);
        }
        public async Task<ServiceResult<ItemResult>> Create(ItemCreateRequest req)
        {
            var e = new Item
            {
                Name = req.Name,
                Description = req.Description,
                Price = req.Price,
                Fuel = req.Fuel,
                Attack = req.Attack,
                Defense = req.Defense,
                ActionCooldownSeconds = req.ActionCooldownSeconds
            };
            _dbContext.Items.Add(e);
            await _dbContext.SaveChangesAsync();

            var dto = await _dbContext.Items
                .Where(i => i.Id == e.Id)
                .ProjectToResult()
                .SingleAsync();

            return new ServiceResult<ItemResult>(dto);
        }

        public async Task<ServiceResult<ItemResult>> Update(ItemUpdateRequest req)
        {
            var e = await _dbContext.Items.FindAsync(req.Id);
            if (e == null)
            {
                var notFound = new ServiceResult<ItemResult>();
                notFound.NotFound(); 
                return notFound;
            }

            e.Name = req.Name;
            e.Description = req.Description;
            e.Price = req.Price;
            e.Fuel = req.Fuel;
            e.Attack = req.Attack;
            e.Defense = req.Defense;
            e.ActionCooldownSeconds = req.ActionCooldownSeconds;
            await _dbContext.SaveChangesAsync();

            var dto = await _dbContext.Items
                .Where(i => i.Id == e.Id)
                .ProjectToResult()
                .SingleAsync();

            return new ServiceResult<ItemResult>(dto);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var e = await _dbContext.Items.FindAsync(id);
            if (e == null) return new ServiceResult().NotFound();

            _dbContext.Items.Remove(e);
            await _dbContext.SaveChangesAsync();
            return new ServiceResult();
        }
    }
}
