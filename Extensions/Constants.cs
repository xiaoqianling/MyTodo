namespace Todo.Extensions
{
    public class Constants
    {
        public const string DbFilename = "NewTodoSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DbPath
        {
            get
            {
                //var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var basePath = AppContext.BaseDirectory;
                return Path.Combine(basePath, DbFilename);
            }
        }
    }
}
