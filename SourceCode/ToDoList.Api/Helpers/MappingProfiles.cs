using AutoMapper;
using ToDoList.Api.Dtos.Entities;
using ToDoList.Core.Entities;

namespace ToDoList.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ToDoNote, ToDoNoteDto>().ReverseMap();
        }
    }
}
