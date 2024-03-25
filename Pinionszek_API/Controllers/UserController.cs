using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;
using Pinionszek_API.Services.DatabaseServices.UserService;
using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApiService _userService;
        private readonly IMapper _mapper;

        public UserController(IConfiguration _config, IUserApiService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user friends by user ID 
        /// </summary>
        /// <param name="idUser">User ID</param>
        [HttpGet("friends")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserFriendDto>))]
        public async Task<IActionResult> GetUserFriendsAsync([Required] int idUser)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var userFriendsData = await _userService.GetUserFriends(idUser);
            if (userFriendsData == null || userFriendsData.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetUserFriendDto>>(userFriendsData));
        }

    }
}
