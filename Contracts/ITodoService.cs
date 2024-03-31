namespace Todo.Contracts
{
    public interface ITodoService
    {
        Task<List<TodoModel>> GetTodos();
        Task<TodoModel> GetTodo(int id);
        Task<int> DeleteTodo(int id);
        Task<int> SaveTodo(TodoModel todoTable);
    }
}
