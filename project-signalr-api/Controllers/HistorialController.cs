using Microsoft.AspNetCore.Mvc;

namespace project_signalr_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistorialController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
