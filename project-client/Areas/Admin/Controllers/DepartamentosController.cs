using Microsoft.AspNetCore.Mvc;

namespace project_client.Areas.Admin.Controllers;

public class DepartamentosController : Controller
{
    public IActionResult Agregar()
    {
        return View();
    }
}
