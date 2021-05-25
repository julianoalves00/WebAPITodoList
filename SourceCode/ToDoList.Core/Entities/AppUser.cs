using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Core.Entities
{
    public class AppUser : BaseEntity
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }
    }
}
