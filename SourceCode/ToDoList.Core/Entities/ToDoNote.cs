using System.Collections.Generic;

namespace ToDoList.Core.Entities
{
    /// <summary>
    /// To do note entity
    /// </summary>
    /// <seealso cref="ToDoList.Core.Entities.BaseEntity" />
    public class ToDoNote : BaseEntity
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public List<string> Items { get; set; }
    }
}
