using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Converters;
using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Repositories;

namespace project_signalr_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TurnoController(TurnoRepository repository) : ControllerBase
{
    readonly TurnoRepository repository = repository;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await repository.GetById(id);

        if (entity is null) return NotFound();

        var response = entity.ToResponse();
        
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await repository.GetAll();

        var response = entities.Select(entity => entity.ToResponse());

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTurnoRequest request)
    {
        var entity = request.ToEntity();

        entity.Fecha = DateTime.UtcNow;
        entity.Estado = "Pendiente";

        await repository.Insert(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToResponse());
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTurnoRequest request)
    {
        var entity = await repository.GetById(request.Id);

        if (entity is null) return NotFound();

        entity.Estado = request.Estado ?? "Pendiente";

        await repository.Update(entity);

        return NoContent();
    }
}
