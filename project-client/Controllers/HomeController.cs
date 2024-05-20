using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using project_client.Models;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace project_client.Controllers;

public class HomeController(HttpClient httpClient) : Controller
{
    private readonly HttpClient client = httpClient;

    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity is null || User.Identity.IsAuthenticated is false) 
            return RedirectToAction(nameof(Login));

        if (User.IsInRole("Admin")) 
            return RedirectToAction("Index", "Home", new { area = "Admin" });

        return RedirectToAction("Index", "Home", new { area = "Usuario" });
    }

    [HttpGet("/iniciar-sesion")]
    [HttpGet("/home/iniciar-sesion")]
    public IActionResult Login() => View();

    [HttpPost("/iniciar-sesion")]
    [HttpPost("/home/iniciar-sesion")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        client.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var json = JsonSerializer.Serialize(viewModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/login", content);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
            else
                ModelState.AddModelError(string.Empty, "Error en la autenticación");

            return View(viewModel);
        }

        var token = await response.Content.ReadAsStringAsync();

        // Agregar el token a las cabeceras de la petición
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Decodificar el token para obtener las reclamaciones
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");
        var name = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name");
        var id = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");

        if (role is null || name is null || id is null)
        {
            ModelState.AddModelError(string.Empty, "Error en la autenticación");
            return View(viewModel);
        }

        var claims = new List<Claim>
            {
                new (ClaimTypes.Role, role.Value),
                new (ClaimTypes.NameIdentifier, id.Value),
                new (ClaimTypes.Name, name.Value),
                new (ClaimTypes.UserData, token)
            };

        var identity = new ClaimsIdentity(claims, "login");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        if (role.Value == "Admin")
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        return RedirectToAction("Index", "Home", new { area = "Usuario" });
    }

    [HttpGet("/cerrar-sesion")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}
