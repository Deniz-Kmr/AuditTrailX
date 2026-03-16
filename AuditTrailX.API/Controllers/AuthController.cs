using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuditTrailX.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    
    // bu endpoint kullanıcı adı ve şifre alır doğrular ve geçerliyse jwt token döner
    [HttpPost("token")]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        // burada sadece test için sabit kullanıcı bilgisi kontrol ediyorum
        // asıl projede veritabanından kullanıcı doğrulaması yapılır...
        if (request.Username != "admin" || request.Password != "admin123")
            return Unauthorized();

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "admin-id"),
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        // token oluşturuyorum issuer audience ve expiration bilgilerini appsettings'ten alıyorum
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["JwtSettings:ExpirationMinutes"]!)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record TokenRequest(string Username, string Password);