using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiEfCodeFirst.Dtos;
using WebApiEfCodeFirst.Jwt;
using WebApiEfCodeFirst.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthController(ApplicationDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Username);

        if (user == null)
            return Unauthorized();

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, model.Password);

        if (result != PasswordVerificationResult.Success)
            return Unauthorized();

        var token = GenerateJwtToken(user.Email);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegister model)
    {
        // Controlla se esiste già un utente con lo stesso username
        var existingUser = await _context.Users
            .SingleOrDefaultAsync(u => u.Email == model.Email);

        if (existingUser != null)
        {
            return BadRequest(new { message = "Username già in uso." });
        }

        // Crea nuovo utente
        var user = new User
        {
            Username = model.Username,
            Email = model.Email
        };

        // Hasher per la password
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, model.Password);

        // Salva nel DB
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Utente registrato con successo." });
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
