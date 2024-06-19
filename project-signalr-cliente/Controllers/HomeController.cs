using Microsoft.AspNetCore.Mvc;

namespace project_signalr_cliente.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PedirTurno()
        {
            return View();
        }
    }
}
