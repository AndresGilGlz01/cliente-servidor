using Microsoft.AspNetCore.Mvc;

using project_signalr_administrador.Models.ViewModel.Home;

namespace project_signalr_administrador.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
    readonly HttpClient httpClient = httpClientFactory.CreateClient("server");

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginRequest)
    {
        var request = new Models.DTOs.Request.LoginRequest
        {
            Username = loginRequest.NombreUsuario,
            Password = loginRequest.Contraseña
        };

        var response = await httpClient.PostAsJsonAsync("api/login", request);

        var responseContext = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, responseContext);

            return View();
        }



        return View();
    }
}
