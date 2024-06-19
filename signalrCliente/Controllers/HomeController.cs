using Microsoft.AspNetCore.Mvc;
using signalrCliente.Models;
using signalrCliente.Services;

namespace signalrCliente.Controllers
{
    public class HomeController : Controller
    {
        TicketService _service;
        public HomeController(TicketService service)
        {
            _service = service;
            //_service.AlSolicitarTurno += _service_AlSolicitarTurno;
        }

       

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
