using System.Threading.Tasks;
using API.DTO;
using API.Services;
using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenservice;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenservice = tokenservice;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email).ConfigureAwait(false);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false).ConfigureAwait(false);

            if(result.Succeeded)
            {
                return new UserDto
                {
                    UserName = user.UserName,
                    Token = _tokenservice.CreateToken(user)
                };
            }

            return Unauthorized();
        }
    }
}