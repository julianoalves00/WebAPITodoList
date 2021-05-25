using System;

namespace ToDoList.Core.Interfaces
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime Timestamp { get; set; }
    }
}
