using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using project_client.Areas.Usuario.Models;
using project_client.Helpers;

using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace project_client.Areas.Usuario.Controllers;

[Area("Usuario")]
[Authorize(Roles ="Usuario")]
public class HomeController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly HttpClient httpClient = httpClient;
    private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? departamento, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
    {
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var reponseActividades = httpClient.GetAsync($"/api/actividades/{userid}");
        var responseDepartamentos = httpClient.GetAsync($"/api/departamentos/{userid}");

        Task.WaitAll(reponseActividades, responseDepartamentos);

        // Verificar si hay error de autenticación
        if (reponseActividades.Result.StatusCode == HttpStatusCode.Unauthorized ||
            responseDepartamentos.Result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("iniciar-sesion", "home", new { area = "" });
        }

        var contentActividades = await reponseActividades.Result.Content.ReadAsStringAsync();
        var contentDepartamentos = await responseDepartamentos.Result.Content.ReadAsStringAsync();

        var actividades = JsonConvert.DeserializeObject<IEnumerable<Models.IndexViewModel.ActividadModel>>(contentActividades) ?? [];
        var departamentos = JsonConvert.DeserializeObject<IEnumerable<Models.IndexViewModel.DepartamentoModel>>(contentDepartamentos) ?? [];

        // Filtrar actividades
        if (departamento != null) actividades = actividades.Where(act => act.Departamento == departamento);
        if (fechaInicio != null) actividades = actividades.Where(act => act.FechaRealizacion != null && act.FechaRealizacion.Value.ToDateTime(TimeOnly.MinValue) >= fechaInicio);
        if (fechaFin != null) actividades = actividades.Where(act => act.FechaRealizacion != null && act.FechaRealizacion.Value.ToDateTime(TimeOnly.MinValue) <= fechaFin);

        var viewModel = new Models.IndexViewModel
        {
            Actividades = actividades.Where(act => act.Estado != 0),
            Departamentos = departamentos,
            Token = token
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Agregar()
    {
        AgregarActividadViewModel actividadViewModel = new AgregarActividadViewModel();
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (vm != null)
        {

            if (vm.IdDepartamento == 0 || vm.IdDepartamento == null)
            {
                var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                vm.IdDepartamento = int.Parse(userid);
            }


            var converter = new ConverterToBase64(webHostEnvironment);

            // Suponiendo que tienes una propiedad 'Imagen' en tu ViewModel que contiene la imagen como un byte array
            // Aquí debes reemplazar 'vm.Imagen' con la propiedad real que contiene la imagen en tu ViewModel
            var imagenBase64 = "";
            if (vm.Archivo != null)
            {

                var ruta = converter.SaveFile(vm.Archivo);
                imagenBase64 = converter.ImageToBase64(ruta);
            }
            if (vm.FechaCreacion == DateTime.MinValue)
            {
                vm.FechaCreacion=DateTime.UtcNow;
            }

            var actdto = new AddActDto()
            {
                Titulo = vm.Titulo,
                FechaActualizacion = vm.FechaActualizacion,
                Descripcion = vm.Descripcion,
                FechaCreacion = vm.FechaCreacion,
                FechaRealizacion = vm.FechaRealizacion,
                IdDepartamento = vm.IdDepartamento ?? 0,
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
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", error);
                return View(vm);
            }
        }
        return View(vm);
    }
}
