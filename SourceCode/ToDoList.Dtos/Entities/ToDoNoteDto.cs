using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos.Entities
{
    public class ToDoNoteDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string Title { get; set; }
        
        public List<string> Items { get; set; }
    }
}
