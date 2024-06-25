using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;
using project_signalr_api.Validators;

namespace project_signalr_api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class UsuarioController(UsuarioRepository repository, CreateUsuarioRequestValidator validator) : ControllerBase
{
    readonly UsuarioRepository usuarioRepository = repository;
    readonly CreateUsuarioRequestValidator validator = validator;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUsuarioRequest usuario)
    {
        var validationResult = await validator.ValidateAsync(usuario);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var entity = new Models.Entities.Administrador
        {
            NombreUsuario = usuario.Nombre!,
            Contraseña = usuario.Contrasena!,
        };

        var result = await usuarioRepository.Insert(entity);

        return Ok();
    }
}
