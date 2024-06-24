using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;
using project_signalr_api.Validators;

namespace project_signalr_api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class UsuarioController(UsuarioRepository repository) : ControllerBase
{
    readonly UsuarioRepository usuarioRepository = repository;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUsuarioRequest usuario)
    {
        var entity = new Models.Entities.Administrador
        {
            NombreUsuario = usuario.Nombre!,
            Contraseña = usuario.Contraseña!,
        };

        var result = await usuarioRepository.Insert(entity);

        return Ok();
    }
}
