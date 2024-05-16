using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using project_client.Areas.Admin.Models;
using System.Security.Claims;

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
    [HttpGet]
    public async Task<IActionResult> Agregar()
    {
        AgregarActividadViewModel actividadViewModel = new AgregarActividadViewModel();
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");
       
        var userid=User.Claims.First(x=>x.Type==ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/Departamentos/{userid}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
           
            var depas = JsonConvert.DeserializeObject<List<Departamentos>>(content);
            if(depas != null)
            {
                actividadViewModel.Departamentos = depas;
            return View(actividadViewModel);
            }
           
        }
        return View(null);
    }

    [HttpPost]
    public IActionResult Agregar(AgregarActividadViewModel vm)
    {

        return View(vm);
    }
}
