using Microsoft.AspNetCore.Mvc;

using project_signalr_administrador.Helpers;
using project_signalr_administrador.Models.DTOs.Response;
using project_signalr_administrador.Models.ViewModel.Home;

using System.Net.Http.Headers;

namespace project_signalr_administrador.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
    readonly HttpClient httpClient = httpClientFactory.CreateClient("server");

    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Session.GetString("token");

        if (string.IsNullOrEmpty(token)) return RedirectToAction(nameof(Login));

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var responses = await httpClient.GetFromJsonAsync<IEnumerable<TurnoResponse>>("api/turno") ?? [];

        var models = responses.Select(t => t.ToModel());

        var viewModel = new IndexViewModel
        {
            Turnos = models
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Historial()
    {
        var token = HttpContext.Session.GetString("token");

        if (string.IsNullOrEmpty(token)) return RedirectToAction(nameof(Login));

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var responses = await httpClient.GetFromJsonAsync<IEnumerable<HistorialResponse>>("api/historial") ?? [];

        var models = responses.Select(t => t.ToModel());

        var viewModel = new HistorialViewModel
        {
            Turnos = models
        };

        return View(viewModel);
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

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, responseContent);

            return View(loginRequest);
        }

        var token = responseContent; 

        HttpContext.Session.SetString("token", token);

        return RedirectToAction(nameof(Index));
    }
}
