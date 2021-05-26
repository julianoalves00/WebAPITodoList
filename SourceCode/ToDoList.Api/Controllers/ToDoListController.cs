using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Dtos.Entities;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IGenericRepository<ToDoNote> _toDoNoteRepo;
        private readonly IGenericRepository<AppUser> _appUserRepo;
        private readonly IMapper _mapper;

        public ToDoListController(IGenericRepository<ToDoNote> toDoNoteRepo, IGenericRepository<AppUser> appUserRepo, IMapper mapper)
        {
            _toDoNoteRepo = toDoNoteRepo;
            _appUserRepo = appUserRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ToDoNoteDto> GetAll(string email)
        {
            List<ToDoNote> list = null;

            if (string.IsNullOrEmpty(email))
                list = _toDoNoteRepo.Get();
            else
                list = _toDoNoteRepo.Get(e => e.Email == email);

            return _mapper.Map<List<ToDoNote>, List<ToDoNoteDto>>(list);
        }

        [HttpGet("{id}")]
        public ToDoNoteDto GetById(int id)
        {
            ToDoNote entity = _toDoNoteRepo.GetById(new ToDoNote() { Id = id });

            return _mapper.Map<ToDoNote, ToDoNoteDto>(entity);
        }

        [HttpPost]
        public ActionResult<ToDoNoteDto> Create(ToDoNoteDto entity)
        {
            if(!ModelState.IsValid)
                return BadRequest("One or more required fields not found.");

            List<AppUser> users = _appUserRepo.Get(f => f.Email == entity.Email);

            if(users == null || users.Count != 1)
                return BadRequest("App user not exists.");

            ToDoNote toDoNote = _mapper.Map<ToDoNoteDto, ToDoNote>(entity);

            ToDoNoteDto toDoNoteCreated = _mapper.Map <ToDoNote, ToDoNoteDto> (_toDoNoteRepo.Create(toDoNote));

            if (toDoNoteCreated == null)
                return BadRequest("Problem create todo note.");

            return Ok(toDoNoteCreated);
        }

        [HttpPut]
        public void Update(ToDoNoteDto entity)
        {
            if (!ModelState.IsValid)
                BadRequest("One or more required fields not found.");

            ToDoNote toDoNote = _mapper.Map<ToDoNoteDto, ToDoNote>(entity);

            _toDoNoteRepo.Update(toDoNote);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            ToDoNote entity = _toDoNoteRepo.GetById(new ToDoNote() { Id = id });

            if (entity == null)
                BadRequest("ToDo List not exists.");

            _toDoNoteRepo.Delete(new ToDoNote() { Id = id } );
        }
    }
}
