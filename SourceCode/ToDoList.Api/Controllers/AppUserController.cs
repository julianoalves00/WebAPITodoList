using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Core.Entities;
using ToDoList.Core.Interfaces;
using ToDoList.Dtos.Entities;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IGenericRepository<AppUser> _appUserRepo;
        private readonly IMapper _mapper;

        public AppUserController(IGenericRepository<AppUser> appUserRepo, IMapper mapper)
        {
            _appUserRepo = appUserRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<AppUserDto> Get()
        {
            List<AppUser> list = _appUserRepo.Get();

            return _mapper.Map<List<AppUser>, List<AppUserDto>>(list);
        }
        [HttpGet("{email}")]
        public AppUserDto GetByEmail(string email)
        {
            List<AppUser> users = _appUserRepo.Get(f => f.Email == email);

            if (users == null || users.Count != 1)
            {
                BadRequest($"App user '{email}' not exists.");
                return null;
            }
            return _mapper.Map<AppUser, AppUserDto>(users.First());
        }

        [HttpPost]
        public ActionResult<AppUserDto> Create(AppUserDto entity)
        {
            List<AppUser> users = _appUserRepo.Get(f => f.Email == entity.Email);

            if (users != null && users.Count != 0)
                return BadRequest($"App user '{entity.Email}' already exists.");

            AppUser AppUser = _mapper.Map<AppUserDto, AppUser>(entity);

            AppUserDto AppUserCreated = _mapper.Map<AppUser, AppUserDto>(_appUserRepo.Create(AppUser));

            if (AppUserCreated == null)
                return BadRequest("Problem create app user.");

            return Ok(AppUserCreated);
        }

        [HttpDelete]
        public void Delete(string email)
        {
            List<AppUser> users = _appUserRepo.Get(f => f.Email == email);

            if (users == null || users.Count != 1)
                BadRequest($"App user '{email}' not exists.");

            AppUser user = users.First();

            if (users == null)
                BadRequest($"Problem try delete app user '{email}'.");

            _appUserRepo.Delete(new AppUser() { Id = user.Id });
        }
    }
}
