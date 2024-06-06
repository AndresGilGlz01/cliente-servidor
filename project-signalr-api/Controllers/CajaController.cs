using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Converters;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;

namespace project_signalr_api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class CajaController(CajaRepository cajaRepository) : ControllerBase
{
    private readonly CajaRepository repository = cajaRepository;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
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
    public async Task<IActionResult> Create()
    {
        var lastNumber = await repository.GetLastCaja() ?? 0;

        var entity = new Caja { NumeroCaja = lastNumber + 1 };

        await repository.Insert(entity);

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await repository.GetById(id);

        if (entity is null) return NotFound();

        await repository.Delete(entity);

        return NoContent();
    }
}
