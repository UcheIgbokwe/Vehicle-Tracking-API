using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Services;
using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
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
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = _tokenservice.CreateToken(user)
                };
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email).ConfigureAwait(false)){
                return BadRequest("Email taken");
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password).ConfigureAwait(false);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem registering user!");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email)).ConfigureAwait(false);

            return CreateUserObject(user);
        }

        private UserDto CreateUserObject(User user)
        {
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenservice.CreateToken(user),
                Id = user.Id
            };
        }
    }
}