using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Dtos.Entities
{
    public class ToDoNoteDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public List<string> Items { get; set; }
    }
}
