using LiteDB;

namespace ToDoList.LiteDB.Interfaces
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}
