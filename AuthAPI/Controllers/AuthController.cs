using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AuthAPI.Models;

namespace BloggingApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _authService.Login(model);
                if (status == 0)
                    return BadRequest(message);
                return Ok(message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(SignInModel model)
        {
            try
            {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.CreateUser(model, UserRoles.Admin);
            if (status == 0)
            {
                return BadRequest(message);
            }
            return CreatedAtAction(nameof(CreateUser), model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}