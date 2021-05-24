using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Interfaces;

namespace ToDoList.Core.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
