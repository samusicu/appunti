using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin input)
        {
            var token = await _authService.LoginAsync(input);
            if (token == null)
                return Unauthorized(new { message = "Credenziali non valide." });

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister input)
        {
            var success = await _authService.RegisterAsync(input);
            if (!success)
                return BadRequest(new { message = "Username già in uso." });

            return Ok(new { message = "Utente registrato con successo." });
        }
    }

}
