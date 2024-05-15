using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using project_client.Models;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace project_client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;


        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;

        }
        [HttpGet]

        public IActionResult Index()
        {
            
                    return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel login)
        {
            var client=new HttpClient();
            client.BaseAddress = new Uri("https://sga.api.labsystec.net/");
            var loginjson = JsonSerializer.Serialize(login);
            var content = new StringContent(loginjson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/login", content);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    
                   
                    ModelState.AddModelError("", "Credenciales incorrectas"); // A�ade el mensaje de error al ModelState
                    return View(login); // Renderiza la vista de error
                }

            }
            else
            {
                var token = await response.Content.ReadAsStringAsync();
                HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true });

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                foreach (var claim in jwtToken.Claims)
                {
                    Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
                var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

                if (roleClaim == null)
                {
                    // Si no se encuentra una reclamaci�n de rol, maneja el error apropiadamente.
                    ModelState.AddModelError("", "No se encontr� ninguna reclamaci�n de rol en el token.");
                    return View(login);
                }

                var role = roleClaim.Value;

                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
                else if (role == "Usuario")
                {
                    return RedirectToAction("UserAction", "User", new { area = "Usuario" });
                }
            }

            return RedirectToAction("Index");
           
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
