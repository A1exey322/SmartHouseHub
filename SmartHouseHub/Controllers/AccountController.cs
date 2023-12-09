using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHouseHub.DAL.Interfaces;
using SmartHouseHub.Domain.Identity;
using SmartHouseHub.Models;
using SmartHouseHub.Service.Interfaces;

namespace SmartHouseHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;

        public AccountController(IBaseRepository<User> userRepository, ITokenProvider tokenprovider, IUserService userService)
        {
            _tokenProvider = tokenprovider ?? throw new ArgumentNullException(nameof(tokenprovider));
            _userService = userService ?? throw new ArgumentNullException( nameof(userService));
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestModel model)
        {

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(ModelState);
            }
               
            if (ModelState.IsValid)
            {
                var response = await _userService.Register(model);
                if (response.StatusCode == Domain.Enums.StatusCode.OK)
                {

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);

        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(UserRequestModel model)
        {

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(ModelState);
            }

            var response = await _tokenProvider.CreateToken(model);
           
            if(response.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest();


        }
    }
}
