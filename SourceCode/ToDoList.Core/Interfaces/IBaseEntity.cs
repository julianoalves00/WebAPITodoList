using System;

namespace ToDoList.Core.Interfaces
{
    /// <summary>
    /// Basic entity interface
    /// </summary>
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime Timestamp { get; set; }
    }
}
