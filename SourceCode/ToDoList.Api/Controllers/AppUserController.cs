using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;
using ToDoList.Dtos.Entities;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IGenericRepository<AppUser> _repositoryAppUser;
        private readonly IMapper _mapper;

        public AppUserController(IGenericRepository<AppUser> repositoryAppUser, IMapper mapper)
        {
            _repositoryAppUser = repositoryAppUser;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<AppUserDto> Get()
        {
            List<AppUser> list = _repositoryAppUser.GetAll();

            return _mapper.Map<List<AppUser>, List<AppUserDto>>(list);
        }
        [HttpGet("{email}")]
        public AppUserDto GetByEmail(string email)
        {
            List<AppUser> users = _repositoryAppUser.GetByFilter(f => f.Email == email);

            if (users == null || users.Count != 1)
            {
                BadRequest("Problem get app user by email.");
                return null;
            }

            return _mapper.Map<AppUser, AppUserDto>(users.First());
        }

        [HttpPost]
        public ActionResult<AppUserDto> Create(AppUserDto entity)
        {
            AppUser AppUser = _mapper.Map<AppUserDto, AppUser>(entity);

            AppUserDto AppUserCreated = _mapper.Map<AppUser, AppUserDto>(_repositoryAppUser.Create(AppUser));

            if (AppUserCreated == null)
                return BadRequest("Problem create app user.");

            return Ok(AppUserCreated);
        }
    }
}
