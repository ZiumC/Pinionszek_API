using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.Services.DatabaseServices.UserService;

namespace Pinionszek_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApiService _userService;
        private readonly IMapper _mapper;

        public UserController(IConfiguration _config, IUserApiService budgetService, IMapper mapper)
        {
            _userService = budgetService;
            _mapper = mapper;
        }
    }
}
