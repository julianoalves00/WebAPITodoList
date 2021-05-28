using LiteDB;
using Microsoft.Extensions.Options;
using ToDoList.LiteDB.Interfaces;

namespace ToDoList.LiteDB
{
    public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public LiteDbContext(IOptions<LiteDbOptions> options)
        {
            Database = new LiteDatabase(options?.Value?.DatabaseLocation);
        }
    }
}
