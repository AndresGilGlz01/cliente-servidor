using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_client.Areas.Usuario.Models;
using System.Security.Claims;

namespace project_client.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    [Authorize(Roles ="Usuario")]
    public class HomeController : Controller
    {

        private readonly HttpClient httpClient;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeController(HttpClient httpClient,IWebHostEnvironment webHostEnvironment)
        {
            this.httpClient = httpClient;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> IndexAsync()
        {
            httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");
            var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var response = await httpClient.GetAsync($"/api/actividades/{userid}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON en una lista de ActividadesViewModel
                var actividades = JsonConvert.DeserializeObject<List<ActividadesViewModel>>(content);
                if (actividades != null)
                {

                    List<ActividadesViewModel> acts = actividades.ToList();
                    return View(acts);
                }
            }
            return View(null);
           
        }
    }
}
