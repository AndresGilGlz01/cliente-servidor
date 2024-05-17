using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using project_client.Areas.Admin.Models;

using System.Net.Http;
using System.Security.Claims;

namespace project_client.Areas.Admin.Controllers;

[Area("Admin")]
public class DepartamentosController : Controller
{
    readonly HttpClient httpClient = new();

    public async Task<IActionResult> Agregar()
    {
        var viewModel = new AgregarDepartamentoViewModel();

        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response = await httpClient.GetAsync($"/api/Departamentos/{userid}");

        if (!response.IsSuccessStatusCode) return View();

        var content = await response.Content.ReadAsStringAsync();

        var departamentos = JsonConvert.DeserializeObject<IEnumerable<Departamentos>>(content);

        if (departamentos == null) return View();

        viewModel.Departamentos = departamentos;

        return View(viewModel);
    }
}
