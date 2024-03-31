using SQLite;

namespace Todo.Model
{
    [Table("todo")]
    public class TodoModel
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("id")]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Column("color")]
        public int Color { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_complete")]
        public bool IsComplete { get; set; }
        public int SprintID { get; set; }

        public override string ToString()
        {
            return $"ID:{Id} Name:{Name} IsCompleted:{IsComplete}";
        }
    }
}
