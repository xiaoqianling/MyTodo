using Microsoft.EntityFrameworkCore;
using Todo.Context;

namespace Todo.Repository
{
    public class SprintRepository : ISprintRepository
    {
        private readonly TodoContext _dbContext;

        public SprintRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteItem(int id)
        {
            var sprint = await _dbContext.Sprints.SingleOrDefaultAsync(x => x.Id == id);
            if (sprint != null)
            {
                _dbContext.Remove(sprint);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<SprintModel?> GetItem(int id)
        {
            return _dbContext.Sprints.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<SprintModel>> GetItems()
        {
            return _dbContext.Sprints.Select(t => t).ToListAsync();
        }

        public async Task<SprintModel?> UpdateItem(SprintModel item)
        {
            var sprint = await _dbContext.Sprints.SingleOrDefaultAsync(x => x.Id == item.Id);
            if (sprint == null)
            {
                return sprint;
            }

            sprint.SprintDuration = item.SprintDuration;
            await _dbContext.SaveChangesAsync();
            return sprint;
        }

        public async Task<SprintModel> SaveItem(SprintModel item)
        {
            var sprint = await _dbContext.Sprints.SingleOrDefaultAsync(x => x.Id == item.Id);
            if (sprint != null)
            {
                sprint.SprintDuration = item.SprintDuration;
            }
            else
            {
                sprint = item;
                await _dbContext.Sprints.AddAsync(sprint);
            }
            await _dbContext.SaveChangesAsync();
            return sprint;
        }
    }
}
