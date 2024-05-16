using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using project_client.Areas.Admin.Models;
using project_client.Helpers;
using System.Security.Claims;
using System.Text;


namespace project_client.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class HomeController : Controller
{
    private readonly HttpClient httpClient;
    private readonly IWebHostEnvironment webHostEnvironment;

    public HomeController(HttpClient httpClient, IWebHostEnvironment webHost)
    {
        this.httpClient = httpClient;
        webHostEnvironment = webHost;
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
            if (actividades != null)
            {

                List<ActividadesViewModel> acts = actividades.ToList();
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

        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/Departamentos/{userid}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var depas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Departamentos>>(content);
            if (depas != null)
            {
                actividadViewModel.Departamentos = depas;
                return View(actividadViewModel);
            }

        }
        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> AgregarAsync(AgregarActividadViewModel vm)
    {
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");
        if (vm != null)
        {

            if (vm.IdDepartamento == 0)
            {
                var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                vm.IdDepartamento = int.Parse(userid);
            }


            var converter = new ConverterToBase64(webHostEnvironment);

            // Suponiendo que tienes una propiedad 'Imagen' en tu ViewModel que contiene la imagen como un byte array
            // Aquí debes reemplazar 'vm.Imagen' con la propiedad real que contiene la imagen en tu ViewModel
            var imagenBase64="";
            if (vm.Archivo != null)
            {

                var ruta = converter.SaveFile(vm.Archivo);
                imagenBase64=converter.ImageToBase64(ruta);
            }
            
            
            var actdto = new AddActDto()
            {
                Titulo = vm.Titulo,
                FechaActualizacion = vm.FechaActualizacion,
                Descripcion = vm.Descripcion,
                FechaCreacion = vm.FechaCreacion,
                FechaRealizacion = vm.FechaRealizacion,
                IdDepartamento = vm.IdDepartamento,
                Estado = 0,
                Imagen = imagenBase64,
                Id = 0
            };
            var loginjson = System.Text.Json.JsonSerializer.Serialize(actdto);
            var content = new StringContent(loginjson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Actividades", content);
            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Index");
            }
            else
            {
                var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var rresponse = await httpClient.GetAsync($"/api/Departamentos/{userid}");
                if (rresponse.IsSuccessStatusCode)
                {
                     var content2 = await rresponse.Content.ReadAsStringAsync();

                    var depas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Departamentos>>(content2);
                    if (depas != null)
                    {
                        vm.Departamentos = depas;
                    }

                }
                var error= await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", error);
                return View(vm);
            }
        }
        return View(vm);
    }
}
