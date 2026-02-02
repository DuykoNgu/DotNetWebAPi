using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Repositories;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepo _tokenRepo;


        public AuthController(UserManager<IdentityUser> userManager, ITokenRepo tokenRepo)
        {
            _userManager = userManager;
            _tokenRepo = tokenRepo;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var identityUser = new IdentityUser
            {
                UserName = dto.UserName,
                Email = dto.UserName,
            };

            var results = await _userManager.CreateAsync(identityUser, dto.Password);

            if (results.Succeeded)
            {
                if (dto.Roles != null && dto.Roles.Any())
                {
                    var roleResults = await _userManager.AddToRolesAsync(identityUser, dto.Roles);
                    if (!roleResults.Succeeded)
                    {
                        return BadRequest("User đã tạo nhưng gán Role thất bại.");
                    }
                }

                return Ok("Đăng ký thành công!");
            }

            return BadRequest(results.Errors);
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user != null)
            {
                var results = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (results)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        
                        var jwtToken = _tokenRepo.CreateJwtToken(user, roles.ToList());
                        var res = new LoginRes
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(res);
                    }
                }
            }

            return BadRequest("Wrong");
        }
    }
}
