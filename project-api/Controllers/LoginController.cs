using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_api.Helpers;
using project_api.Models.Dtos;
using project_api.Repositories;

namespace project_api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class LoginController(DepartamentosRepository departamentosRepository, IConfiguration configuration) : ControllerBase
{
    readonly DepartamentosRepository departamentosRepository = departamentosRepository;
    readonly IConfiguration configuration = configuration;

    [HttpPost]
    public IActionResult Login(LoginDto login)
    {
        if (login.UserName.Contains("equipo3"))
        {

            var departemento = departamentosRepository.Get(login.UserName);

            if (departemento is null) return BadRequest("No hay departamentos");

            var success = Verifier.VerifyPassword(login.Password, departemento.Password);

            if (!success) return BadRequest("Credenciales incorrectas");
        
            string role = departemento.Nombre == "Dirección General - Equipo 3" ? "Admin" : "Usuario";

            var jwttoken = new JwtTokenGenerator(configuration);

            var token = jwttoken.GetToken(departemento, role);

            return Ok(token);
        }
        else
        {
            return BadRequest("Correo incorrecto");
        }
    }
}
