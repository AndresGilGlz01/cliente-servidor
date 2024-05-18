using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_client.Areas.Admin.Models;

using System.Security.Claims;
using System.Text.Json;

namespace project_client.Areas.Admin.Controllers;
[Authorize(Roles ="Admin")]
[Area("Admin")]
public class DepartamentosController : Controller
{
    readonly HttpClient httpClient = new();

    public async Task<IActionResult> IndexAsync()
    {
        DepartamentosViewModel vm= new DepartamentosViewModel();
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");
        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/departamentos/{userid}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON en una lista de ActividadesViewModel
            var depas = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content);
            if (depas != null)
            {

                vm.Departamentos= depas;
                return View(vm);
            }
        }
        return View();
    }

    public async Task<IActionResult> Agregar()
    {
        var viewModel = new AgregarDepartamentoViewModel();

        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/Departamentos/{userid}");

        if (!response.IsSuccessStatusCode) return View();

        var content = await response.Content.ReadAsStringAsync();

        var departamentos = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (departamentos == null) return View();

        viewModel.Departamentos = departamentos;

        return View(viewModel);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var viewModel = new EditarDepartamentoViewModel();

        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var response = await httpClient.GetAsync($"/api/departamento/{id}");

        if (!response.IsSuccessStatusCode) return View();

        var content = await response.Content.ReadAsStringAsync();

        var departamento = JsonSerializer.Deserialize<Departamentos>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (departamento == null) return View();

        viewModel.Id = departamento.Id;
        viewModel.Nombre = departamento.Nombre;
        viewModel.Username = departamento.Username;
        viewModel.DepartamentoSuperior = departamento.DepartamentoSuperior;

        return View(viewModel);
    }
    public IActionResult Eliminar(int id)
    {
        return View();
    }
}
