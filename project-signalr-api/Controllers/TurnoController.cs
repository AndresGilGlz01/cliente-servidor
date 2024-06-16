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
public class TurnoController(TurnoRepository turnoRepository,
    CajaRepository cajaRepository, HistorialRepository historialRepository,
    UpdateTurnoRequestValidator updateTurnoValidator, CreateTurnoRequestValidator createTurnoValidator) : ControllerBase
{
    readonly UpdateTurnoRequestValidator updateTurnoValidator = updateTurnoValidator;
    readonly CreateTurnoRequestValidator createTurnoValidator = createTurnoValidator;
    readonly HistorialRepository historialRepository = historialRepository;
    readonly TurnoRepository turnoRepository = turnoRepository;
    readonly CajaRepository cajaRepository = cajaRepository;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await turnoRepository.GetById(id);

        if (entity is null) return NotFound();

        var response = entity.ToResponse();

        return Ok(response);
    }

    [HttpGet("bycaja/{id}")]
    public async Task<IActionResult> GetByCaja(int id)
    {
        var entity = await turnoRepository.GetByCaja(id);

        if (entity is null) return NotFound();

        var response = entity.ToResponse();

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await turnoRepository.GetAll();

        var response = entities.Select(entity => entity.ToResponse());

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTurnoRequest request)
    {
        var validationResult = await createTurnoValidator.ValidateAsync(request);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));

        var entity = request.ToEntity();

        entity.Fecha = DateTime.UtcNow;
        entity.Estado = "Pendiente";

        await turnoRepository.Insert(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToResponse());
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTurnoRequest request)
    {
        var validationResult = await updateTurnoValidator.ValidateAsync(request);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));

        var turno = await turnoRepository.GetById(request.IdTurno);
        var caja = await cajaRepository.GetById(request.IdCaja);

        var historial = new Historial
        {
            IdTurno = request.IdTurno,
            IdCaja = request.IdCaja,
            Estado = request.Estado!,
            FechaAtencion = DateTime.UtcNow
        };
        await historialRepository.Insert(historial);

        turno!.Estado = request.Estado!;
        await turnoRepository.Update(turno);

        caja!.IdTurnoActual = turno.Estado == "Atendiendo" ? request.IdTurno : null;
        await cajaRepository.Update(caja);

        return NoContent();
    }
}
