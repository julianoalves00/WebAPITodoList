using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Interfaces;

namespace ToDoList.Core.Entities
{
    public class ToDoNote : BaseEntity, IBaseEntity
    {
        public string Title { get; set; }
        public List<string> Items { get; set; }

        public ToDoNote() { }
        public ToDoNote(int id = 0) : base(id) { }
        public ToDoNote(string title, List<string> items = null)
        {
            Title = title;
            Items = items;
        }
    }
}
