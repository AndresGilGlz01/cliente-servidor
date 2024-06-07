using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;
using project_signalr_api.Validators;

using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project_signalr_api.Controllers;

[ApiController]
[Route("api")]
public class HomeController(IConfiguration configuration,
    AdministradorRepository administradorRepository,
    LoginRequestValidator loginRequestValidator) : ControllerBase
{
    readonly AdministradorRepository administradorRepository = administradorRepository;
    readonly LoginRequestValidator loginRequestValidator = loginRequestValidator;
    readonly IConfiguration configuration = configuration;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var validationResult = await loginRequestValidator.ValidateAsync(loginRequest);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));

        var administrador = await administradorRepository.Login(loginRequest.Username!, loginRequest.Password!);

        var token = GenerateJwtToken(administrador!);

        return Ok(token);
    }

    string GenerateJwtToken(Administrador administrador)
    {
        var issuer = configuration["Jwt:Issuer"];
        var key = configuration["Jwt:Key"];

        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, administrador.NombreUsuario),
            new Claim(ClaimTypes.NameIdentifier, administrador.Id.ToString()),
            new Claim(ClaimTypes.Role, "Administrador"),
            new Claim(JwtRegisteredClaimNames.Iss, issuer!),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddDays(1).ToString()),
        ];

        var keyBytes = Encoding.UTF8.GetBytes(key!);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = issuer,
            IssuedAt = DateTime.Now,
            Expires = DateTime.Now.AddDays(1),
            NotBefore = DateTime.Now,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }
}
