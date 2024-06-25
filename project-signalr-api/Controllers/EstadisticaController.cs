using Microsoft.AspNetCore.Mvc;

using project_signalr_api.Models.DTOs.Response;
using project_signalr_api.Models.Entities;
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
        var cajaMasFrecuente = historialRepository.GetCajaMasFrecuenteHoy();
        var cajaMenosFrecuente = historialRepository.GetCajaMenosFrecuenteHoy();

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
    [HttpGet]
    [Route("EstaHora")]
    public async Task<IActionResult> GetEstadisticasPorHora()
    {
        DateTime hora = DateTime.Now;
        var cantidad = await historialRepository.GetAll();
        var s = cantidad.Where(x => x.FechaAtencion.Hour == hora.Hour);
        int volumen = s.Count();

         //int volumen = cantidad.Where(x=>x.FechaAtencion.Hour == 6).Count();
        double espera = 60 / 12;
        var _response = new EstadisticaResponse
        {
            TiempoPromedioDeEspera = $"{espera} minutos por cliente",
            VolumenDeUsuarios = $"A las {hora.Hour} del dia {hora.Day} de {hora.Month} se atendieron a {volumen} usuarios"
        };
        return Ok(_response);
    }
    [HttpGet]
    [Route("Hoy")]
    public async Task<IActionResult> GetEstadisticasPorDia()
    {
        DateTime dia = DateTime.Now;
       var cantidad = await historialRepository.GetAll();
        var s = cantidad.Where(x => x.FechaAtencion.Day == 24);
        int volumen = s.Count();
        double espera = (dia.Hour * 60)/volumen  ;
        var _response = new EstadisticaResponse
        {
            TiempoPromedioDeEspera = $"{espera.ToString("0.0")} minutos por cliente",
            VolumenDeUsuarios = $"El dia {dia.Day} de {dia.Month} se atendieron a {volumen} usuarios"
        };
        return Ok(_response);
    }
}
