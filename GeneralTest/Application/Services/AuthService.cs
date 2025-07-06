using EfCodeFirst.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Application.Settings;
using Microsoft.EntityFrameworkCore;
using Application.Dtos;

public class AuthService
{
    private readonly TestCodeFirstContext _context;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly PasswordHasher<User> _hasher;

    public AuthService(TestCodeFirstContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings;
        _hasher = new PasswordHasher<User>();
    }

    public async Task<string?> LoginAsync(UserLogin input)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == input.Email);
        if (user == null)
            return null;

        var result = _hasher.VerifyHashedPassword(user, user.Password, input.Password);
        if (result != PasswordVerificationResult.Success)
            return null;

        return GenerateJwtToken(user.Email);
    }

    public async Task<bool> RegisterAsync(UserRegister input)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == input.Email);
        if (exists)
            return false;

        var user = new User
        {
            Username = input.Username,
            Email = input.Email
        };
        user.Password = _hasher.HashPassword(user, input.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateJwtToken(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.DurationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
