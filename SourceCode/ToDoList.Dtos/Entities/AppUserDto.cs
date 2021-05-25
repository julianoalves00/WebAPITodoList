using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos.Entities
{
    public class AppUserDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string DisplayName { get; set; } 
    }
}
