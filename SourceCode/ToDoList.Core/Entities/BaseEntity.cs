using System;
using ToDoList.Core.Interfaces;

namespace ToDoList.Core.Entities
{
    /// <summary>
    /// Basic entity
    /// </summary>
    /// <seealso cref="ToDoList.Core.Interfaces.IBaseEntity" />
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        public BaseEntity(int id = 0)
        {
            Id = id;
            Timestamp = DateTime.Now;
        }
    }
}
