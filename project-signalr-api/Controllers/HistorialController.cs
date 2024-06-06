using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Converters;
using project_signalr_api.Repositories;

namespace project_signalr_api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class HistorialController(HistorialRepository historialRepository) : ControllerBase
{
    readonly HistorialRepository historialRepository = historialRepository;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var entity = await historialRepository.GetById(id);

        if (entity == null) return NotFound();

        var response = entity.ToResponse();

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await historialRepository.GetAll();

        var response = entities.Select(entity => entity.ToResponse());

        return Ok(response);
    }
}
