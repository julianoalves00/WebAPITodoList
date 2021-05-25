using System.Collections.Generic;

namespace ToDoList.Core.Entities
{
    public class ToDoNote : BaseEntity
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public List<string> Items { get; set; }
    }
}
