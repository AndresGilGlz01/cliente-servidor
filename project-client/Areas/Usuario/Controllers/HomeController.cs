using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace project_client.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize(Roles ="Usuario")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
