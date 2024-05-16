using Microsoft.AspNetCore.Mvc;

namespace project_api.Controllers;

public class ImageController : Controller
{
    [HttpPost("{id}")]
    public IActionResult UploadImage(int id, [FromBody] string imagen64)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{id}.png");
        var bytes = Convert.FromBase64String(imagen64);
        System.IO.File.WriteAllBytes(path, bytes);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteImage(int id)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{id}.png");
        System.IO.File.Delete(path);
        return Ok();
    }
}
