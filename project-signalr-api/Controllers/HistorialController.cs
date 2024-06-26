﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Converters;
using project_signalr_api.Repositories;

namespace project_signalr_api.Controllers;

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
    public IActionResult GetAll()
    {
        var entities = historialRepository.GetAll().Result
            .OrderByDescending(entity => entity.FechaAtencion)
            .ThenByDescending(x => x.Id);

        var response = entities.Select(entity => entity.ToResponse());

        return Ok(response);
    }
}
