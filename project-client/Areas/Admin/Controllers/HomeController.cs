using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_client.Areas.Admin.Models;

namespace project_client.Areas.Admin.Controllers;

[Authorize(Roles ="Admin")]
[Area("Admin")]
public class HomeController : Controller
{
    private readonly HttpClient httpClient;

    public HomeController(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IActionResult> IndexAsync()
    {
        
        
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");
        var response = await httpClient.GetAsync("/api/actividades");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON en una lista de ActividadesViewModel
            var actividades = JsonConvert.DeserializeObject<List<ActividadesViewModel>>(content);
            if(actividades != null)
            {

            List<ActividadesViewModel> acts=actividades.ToList();
            return View(acts);
            }
        }
        return View(null);
    }
}
