namespace Todo.Model
{
    public class SprintModel
    {
        public int Id { get; set; }

        public int SprintDuration { get; set; }

        public List<TodoModel> TodoItems { get; }

    }
}
