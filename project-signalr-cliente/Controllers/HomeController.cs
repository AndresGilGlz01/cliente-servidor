using Microsoft.AspNetCore.Mvc;
using project_signalr_cliente.Models.Entities;
using project_signalr_cliente.Services;

namespace project_signalr_cliente.Controllers
{
    public class HomeController : Controller
    {
        TicketsService _service;
        Turno? _turno = new();
        public HomeController(TicketsService service)
        {
            _service = service;
            _service.ElfokinEvento += _service_ElfokinEvento;
        }

        private void _service_ElfokinEvento(object? sender, string e)
        {
            _turno.Numero = e;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> TurnoAsync()
        {
            await _service.SolicitarTurno();
            if (!string.IsNullOrWhiteSpace(_turno.Numero))
            {
                return View(_turno);
            }

            return BadRequest("No jalan los turnos");
        }
    }
}
