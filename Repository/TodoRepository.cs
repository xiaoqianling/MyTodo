using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Context;

namespace Todo.Repository
{
    // Todo存储实现
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _dbContext;

        public TodoRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteItem(int id)
        {
            var todo = await _dbContext.Todos.SingleOrDefaultAsync(x => x.Id == id);
            if (todo != null)
            {
                _dbContext.Remove(todo);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<TodoModel?> GetItem(int id)
        {
            return _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<TodoModel>> GetItems()
        {
            return _dbContext.Todos.Select(t => t).ToListAsync();
        }

        public async Task<TodoModel?> UpdateItem(TodoModel item)
        {
            var todo = await _dbContext.Todos.SingleOrDefaultAsync(x => x.Id == item.Id);
            if (todo == null)
            {
                return todo;
            }

            todo.Description = item.Description;
            todo.Name = item.Name;
            todo.IsComplete = item.IsComplete;
            todo.Color = item.Color;

            await _dbContext.SaveChangesAsync();
            return todo;
        }

        public async Task<TodoModel> SaveItem(TodoModel item)
        {
            var todo = await _dbContext.Todos.SingleOrDefaultAsync(x => x.Id == item.Id);
            Debug.WriteLine($"查询到的对象：{todo}");
            if (todo != null)
            {
                todo.Description = item.Description;
                todo.Name = item.Name;
                todo.IsComplete = item.IsComplete;
                todo.Color = item.Color;
            }
            else
            {
                todo = item;
                await _dbContext.Todos.AddAsync(todo);
            }
            Debug.WriteLine($"更改结果：{todo}");
            await _dbContext.SaveChangesAsync();
            return todo;
        }
    }
}
