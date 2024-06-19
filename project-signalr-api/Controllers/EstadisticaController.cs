using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Models.DTOs.Response;
using project_signalr_api.Repositories;

namespace project_signalr_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadisticaController(HistorialRepository historialRepository,
    TurnoRepository turnoRepository) : ControllerBase
{
    readonly HistorialRepository historialRepository = historialRepository;
    readonly TurnoRepository turnoRepository = turnoRepository;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cantidadEspera = await turnoRepository.GetCantidadTurnosEspera();
        var cantidadAtendiendo = await turnoRepository.GetCantidadTurnosAtendiendo();
        var cantidadAtendidos = await turnoRepository.GetCantidadTurnosAtendidos();
        var cajaMasFrecuente = await historialRepository.GetCajaMasFrecuente();
        var cajaMenosFrecuente = await historialRepository.GetCajaMenosFrecuente();

        var responose = new EstadisticaResponse
        {
            CantidadEspera = cantidadEspera,
            CantidadAtendiendo = cantidadAtendiendo,
            CantidadAtendidos = cantidadAtendidos,
            CajaMasFrecuente = cajaMasFrecuente,
            CajaMenosFrecuente = cajaMenosFrecuente
        };

        return Ok(responose);
    }
}
