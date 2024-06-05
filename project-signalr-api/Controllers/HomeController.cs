using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;
using project_signalr_api.Validators;

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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, administrador.Id.ToString()),
                new(ClaimTypes.Name, administrador.NombreUsuario)
            ]),
            Expires = DateTime.UtcNow.AddHours(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
