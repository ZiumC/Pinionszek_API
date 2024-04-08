using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Utils;
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
        private readonly AccountUtils _accountUtils;

        public UserController(IConfiguration _config, IUserApiService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _accountUtils = new AccountUtils(_config);

        }

        /// <summary>
        /// Get user friends by user ID 
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet("{idUser}/friends")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserFriendDto>))]
        public async Task<IActionResult> GetUserFriendsAsync(int idUser, int page = 1, int pageSize = 20)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            if (page <= 0)
            {
                ModelState.AddModelError("error", "Page number is invalid");
                return BadRequest(ModelState);
            }

            if (pageSize <= 0)
            {
                ModelState.AddModelError("error", "Page size is invalid");
                return BadRequest(ModelState);
            }

            var userFriendsData = await _userService.GetUserFriendsAsync(idUser);
            userFriendsData = userFriendsData?
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            if (userFriendsData == null || userFriendsData.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetUserFriendDto>>(userFriendsData));
        }

        /// <summary>
        /// Get user settings by user ID 
        /// </summary>
        /// <param name="idUser">User ID</param>
        [HttpGet("{idUser}/settings")]
        [ProducesResponseType(200, Type = typeof(GetUserSettingsDto))]
        public async Task<IActionResult> GetUserSettingsAsync(int idUser)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var userSettingsData = await _userService.GetUserSettingsAsync(idUser);
            if (userSettingsData == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetUserSettingsDto>(userSettingsData));
        }

        /// <summary>
        /// Get user prodile data by user ID 
        /// </summary>
        /// <param name="idUser">User ID</param>
        [HttpGet("{idUser}")]
        [ProducesResponseType(200, Type = typeof(GetUserProfileDto))]
        public async Task<IActionResult> GetUserProfileDataAsync(int idUser)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var userProfileData = await _userService.GetUserDataAsync(idUser);
            if (userProfileData == null)
            {
                return NotFound();
            }

            string email = _accountUtils.MaskEmailString(userProfileData.Email);
            userProfileData.Email = email;

            return Ok(_mapper.Map<GetUserProfileDto>(userProfileData));
        }

        /// <summary>
        /// Get user payment categories by user ID
        /// </summary>
        /// <param name="idUser">User ID</param>
        [HttpGet("{idUser}/payment-categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetUserCategoryDto>))]
        public async Task<IActionResult> GetPaymentsCategoriesAsync(int idUser)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var userGeneralCategoriesData = await _userService.GetUserPaymentCategoriesAsync(idUser);
            if (userGeneralCategoriesData == null || userGeneralCategoriesData.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetUserCategoryDto>>(userGeneralCategoriesData));
        }

        /// <summary>
        /// Get user payment category by user ID and detailed category ID
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="idDetailedCategory">Detailed category ID</param>
        [HttpGet("{idUser}/payment-categories/{idDetailedCategory}")]
        [ProducesResponseType(200, Type = typeof(GetUserCategoryDto))]
        public async Task<IActionResult> GetPaymentCategoryAsync(int idUser, int idDetailedCategory)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            if (idDetailedCategory <= 0)
            {
                ModelState.AddModelError("error", "Detailed category ID is invalid");
                return BadRequest(ModelState);
            }

            var userGeneralCategoriesData = 
                await _userService.GetUserPaymentCategoryAsync(idUser, idDetailedCategory);
            if (userGeneralCategoriesData == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetUserCategoryDto>(userGeneralCategoriesData));
        }
    }
}
