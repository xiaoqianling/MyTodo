using SQLite;

namespace Todo.Services
{
    public class TodoService : ITodoService
    {
        static SQLiteAsyncConnection Database;

        public static readonly LazyAsync<TodoService> Instance = new LazyAsync<TodoService>(async () =>
        {
            var instance = new TodoService();
            CreateTableResult result = await Database.CreateTableAsync<TodoModel>();
            return instance;
        });

        private TodoService()
        {
            Database = new SQLiteAsyncConnection(Constants.DbPath, Constants.Flags);
        }
        public Task<int> DeleteTodo(int id)
        {
            return Database.DeleteAsync(id);
        }
        public Task<TodoModel> GetTodo(int id)
        {
            return Database.Table<TodoModel>().Where((todo) => todo.Id == id).FirstOrDefaultAsync();
        }
        public Task<List<TodoModel>> GetTodos()
        {
            return Database.Table<TodoModel>().ToListAsync();
        }
        public Task<int> SaveTodo(TodoModel todoTable)
        {

            if (todoTable.Id != 0)
            {
                return Database.UpdateAsync(todoTable);
            }
            return Database.InsertAsync(todoTable);
        }
    }
}
