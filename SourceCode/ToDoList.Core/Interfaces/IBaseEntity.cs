using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Core.Interfaces
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
