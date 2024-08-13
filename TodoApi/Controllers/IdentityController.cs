using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Common.Extensions;
using TodoApi.Models.Auth;
using TodoApi.Service.Auth;
using TodoApi.Service.Identity;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;
        private readonly JwtService _jwtService;

        public IdentityController(
            IdentityService identityService,
            JwtService jwtService
        )
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

#if DEBUG

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _identityService.GetUserAsync(User);
            return user switch
            {
                null => Unauthorized(),
                _ => Ok(user)
            };
        }

#endif

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAuthRequest request)
        {
            if (!request.Validate().IsValid)
                return BadRequest();

            var userCreated = await _identityService.RegisterAsync(request.Email, request.Password);
            if (!userCreated.IsSuccess)
            {
                var errors = userCreated.Error.GetErrorsDescriptions();
                return BadRequest(errors);
            }
            var user = userCreated.Value!;
            var principal = await _identityService.GetClaimsPrincipalAsync(user);

            var tokens = _jwtService.GetNewToken(principal);
            return Ok(tokens);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthRequest request)
        {
            if (!request.Validate().IsValid)
                return BadRequest();

            var user = await _identityService.GetUserAsync(request.Email);
            if (user is null)
            {
                return Unauthorized();
            }
            var principal = await _identityService.GetClaimsPrincipalAsync(user);
            var tokens = _jwtService.GetNewToken(principal);
            return Ok(tokens);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request)
        {
            if (!request.Validate().IsValid)
                return Unauthorized();

            var tokens = _jwtService.GetUpdatedToken(User, request.RefreshToken);
            return tokens switch
            {
                null => Unauthorized(),
                _ => Ok(tokens)
            };
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] UserDeleteRequest request)
        {
            if (!request.Validate().IsValid)
                return BadRequest();

            var user = await _identityService.GetUserAsync(request.Email);
            if (user is null)
                return NotFound();
            if (!_identityService.IsEqual(user, User))
                return Unauthorized();

            await _identityService.RemoveAsync(user);
            return Ok();
        }
    }
}