using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Dtos;
using ToDoList.Api.Dtos.Entities;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Repository;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IGenericRepository<ToDoNote> _toDoNoteRepo;
        private readonly ILogger<ToDoListController> _logger;
        private readonly IMapper _mapper;

        public ToDoListController(IGenericRepository<ToDoNote> toDoNoteRepo, ILogger<ToDoListController> logger, IMapper mapper)
        {
            _toDoNoteRepo = toDoNoteRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ToDoNoteDto> Get()
        {
            List<ToDoNote> list = _toDoNoteRepo.GetAll();

            return _mapper.Map<List<ToDoNote>, List<ToDoNoteDto>>(list);
        }

        [HttpGet("{id}")]
        public ToDoNoteDto GetById(int id)
        {
            ToDoNote entity = _toDoNoteRepo.GetById(new ToDoNote(id));

            return _mapper.Map<ToDoNote, ToDoNoteDto>(entity);
        }

        [HttpPost]
        public ActionResult<ToDoNoteDto> Create(ToDoNoteDto entity)
        {
            ToDoNote toDoNote = _mapper.Map<ToDoNoteDto, ToDoNote>(entity);

            ToDoNoteDto toDoNoteCreated = _mapper.Map <ToDoNote, ToDoNoteDto> (_toDoNoteRepo.Create(toDoNote));

            if (toDoNoteCreated == null)
                return BadRequest("Problem create todo note.");

            return Ok(toDoNoteCreated);
        }

        [HttpPut]
        public void Update(ToDoNoteDto entity)
        {
            ToDoNote toDoNote = _mapper.Map<ToDoNoteDto, ToDoNote>(entity);

            _toDoNoteRepo.Update(toDoNote);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _toDoNoteRepo.Delete(new ToDoNote() { Id = id } );
        }
    }
}
