namespace ToDoList.Core.Entities
{
    /// <summary>
    /// App user entity
    /// </summary>
    /// <seealso cref="ToDoList.Core.Entities.BaseEntity" />
    public class AppUser : BaseEntity
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}
