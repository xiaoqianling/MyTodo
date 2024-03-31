using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Todo.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            //this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //this.ChangeTracker.LazyLoadingEnabled = false;
            Debug.WriteLine($"SQLITE Filename={Constants.DbPath}");
            if (Database.EnsureCreated())
            {
                Debug.WriteLine("NOT EXIST");
            }
            else Debug.WriteLine("数据库已存在");
            SeedData();

        }

        private bool DatabaseExists()
        {
            return File.Exists(Constants.DbPath);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={Constants.DbPath}");
            optionsBuilder.LogTo(message => Debug.WriteLine(message), new[] {
                DbLoggerCategory.Database.Command.Name
            }, LogLevel.Information).EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Debug.WriteLine("OnModelCreating");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoModel>();

            modelBuilder.Entity<SprintModel>();
        }

        private void SeedData()
        {
            Debug.WriteLine("SeedData Configurate...");
            // 添加种子数据到 TodoTable
            if (!Todos.Any())
            {
                Todos.AddRange(
                    new TodoModel { Name = "Task 1", Id = 1, Description = "Description for Task 1", SprintID = 1, IsComplete = false },
                    new TodoModel { Id = 2, Name = "Task 2", Description = "Description for Task 2", SprintID = 1, IsComplete = true },
                    new TodoModel { Id = 3, Name = "Hello", Description = "Description for Task 2", SprintID = 1, IsComplete = false }
                // 添加更多种子数据...
                );
                SaveChanges();
            }
            else Debug.WriteLine("已存在数据，不生成种子数据");

            // 添加种子数据到 SprintTable
            if (!Sprints.Any())
            {
                Sprints.AddRange(
                    new SprintModel { Id = 1 },
                    new SprintModel { Id = 2 }
                // 添加更多种子数据...
                );
                SaveChanges();
            }
        }

        public DbSet<TodoModel> Todos
        {
            get; set;
        }
        public DbSet<SprintModel> Sprints
        {
            get; set;
        }


    }
}
