using Microsoft.AspNetCore.Mvc;

using project_client.Areas.Usuario.Models;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace project_client.Areas.Usuario.Controllers;

public class DepartamentosController : Controller
{
    readonly HttpClient httpClient = new();

    public async Task<IActionResult> IndexAsync()
    {
        DepartamentosViewModel vm = new DepartamentosViewModel();
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/departamentos/{userid}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON en una lista de ActividadesViewModel
            var depas = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (depas != null)
            {

                vm.Departamentos = depas;
                return View(vm);
            }
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Agregar()
    {
        var viewModel = new AgregarDepartamentoViewModel();

        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/Departamentos/{userid}");

        if (!response.IsSuccessStatusCode) return View();

        var content = await response.Content.ReadAsStringAsync();

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Agregar(AgregarDepartamentoViewModel viewModel)
    {
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (viewModel != null)
        {

            var dep = new Departamentos()
            {
                Id = 0,
                Nombre = viewModel.Nombre,
                Username = viewModel.Username,
                Password = viewModel.Password,

            };
            var loginjson = System.Text.Json.JsonSerializer.Serialize(dep);
            var content = new StringContent(loginjson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Departamentos", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", error);
                return View(viewModel);
            }
        }

        //var response = await httpClient.GetAsync($"/api/Departamentos/{userid}");

        //if (!response.IsSuccessStatusCode) return View();

        //var content = await response.Content.ReadAsStringAsync();

        //var departamentos = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //if (departamentos == null) return View();

        //viewModel.Departamentos = departamentos;

        return View(viewModel);
    }
}
