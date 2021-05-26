using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos.Entities
{
    /// <summary>
    /// To do note DTO entity
    /// </summary>
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
