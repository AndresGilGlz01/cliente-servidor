using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using project_client.Models;

using System.Diagnostics;
using System.Net;
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
                    
                   
                    ModelState.AddModelError("", "Credenciales incorrectas"); // Añade el mensaje de error al ModelState
                    return View(login); // Renderiza la vista de error
                }

            }
            return RedirectToAction();
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
