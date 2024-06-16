using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Converters;
using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;
using project_signalr_api.Validators;

namespace project_signalr_api.Controllers;

[Authorize(Roles = "Administrador")]
[ApiController]
[Route("api/[controller]")]
public class CajaController(CajaRepository cajaRepository, UpdateCajaRequestValidator updateCajaRequestValidator) : ControllerBase
{
    private readonly CajaRepository repository = cajaRepository;
    private readonly UpdateCajaRequestValidator updateCajaRequestValidator = updateCajaRequestValidator;

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

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCajaRequest request)
    {
        var validationResult = await updateCajaRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));

        var entity = await repository.GetById(request.IdCaja);

        if (entity is null) return NotFound();

        entity.IdAdministradorActual = request.IdAdministrador;

        await repository.Update(entity);

        return NoContent();
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
