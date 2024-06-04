using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_client.Areas.Admin.Models;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace project_client.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class DepartamentosController : Controller
{
    readonly HttpClient httpClient = new();

    public async Task<IActionResult> IndexAsync()
    {
        var viewModel = new DepartamentosViewModel();
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

                viewModel.Departamentos = depas;
                return View(viewModel);
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

        var response2 = await httpClient.GetAsync($"/api/Departamentos/{userid}");
        if (response2.IsSuccessStatusCode)
        {
            var content2 = await response2.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON en una lista de ActividadesViewModel
            var depas = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (depas != null)
            {

                viewModel.Departamentos = depas;
                return View(viewModel);
            }
        }

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Agregar(AgregarDepartamentoViewModel viewModel)
    {
        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (viewModel != null)
        {
           
            if (viewModel.IdSuperior == 0)
            {

                viewModel.IdSuperior = int.Parse(userid);
            }
            var dep = new Departamentos()
            {
                Id = 0,
                Nombre = viewModel.Nombre,
                Username = viewModel.Username,
                Password = viewModel.Password,
                IdSuperior = viewModel.IdSuperior,

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
            var response2 = await httpClient.GetAsync($"/api/Departamentos/{userid}");
            if (response2.IsSuccessStatusCode)
            {
                var content2 = await response2.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON en una lista de ActividadesViewModel
                var depas = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (depas != null)
                {

                    viewModel.Departamentos = depas;
                    return View(viewModel);
                }
            }
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

    [HttpGet("admin/departamentos/editar/{id}")]
    public async Task<IActionResult> Editar(int id)
    {
        var viewModel = new EditarDepartamentoViewModel();

        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync($"/api/departamento/{id}");

        if (!response.IsSuccessStatusCode) return View();

        var content = await response.Content.ReadAsStringAsync();

        var departamento = JsonSerializer.Deserialize<Departamentos>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (departamento == null) return View();

        viewModel.Id = departamento.Id;
        viewModel.Nombre = departamento.Nombre;
        viewModel.Username = departamento.Username;
        viewModel.IdSuperior = (int)departamento.IdSuperior;
        var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var response2 = await httpClient.GetAsync($"/api/Departamentos/{userid}");
        if (response2.IsSuccessStatusCode)
        {
            var content2 = await response2.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON en una lista de ActividadesViewModel
            var depas = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (depas != null)
            {

                viewModel.Departamentos = depas;
                return View(viewModel);
            }
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(EditarDepartamentoViewModel vm)
    {
        var userid = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response2 = await httpClient.GetAsync($"/api/departamento/{vm.Id}");

        if (!response2.IsSuccessStatusCode) return View();

        var content2 = await response2.Content.ReadAsStringAsync();

        var departamento = JsonSerializer.Deserialize<Departamentos>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (vm.IdSuperior == 0)
        {
            vm.IdSuperior = (int)departamento.IdSuperior; }
        var dto = new EditDepaViewModel()
        {
            Id = vm.Id,
            Nombre = vm.Nombre,
            Username = vm.Username,
            Password = vm.Password,
            IdSuperior = vm.IdSuperior,
        };
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(dto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync("/api/Departamentos", content);


        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");

        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", error);
            var response3 = await httpClient.GetAsync($"/api/Departamentos/{userid}");
            if (response3.IsSuccessStatusCode)
            {
                var content3 = await response3.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON en una lista de ActividadesViewModel
                var depas = JsonSerializer.Deserialize<IEnumerable<Departamentos>>(content3, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (depas != null)
                {

                    vm.Departamentos = depas;
                    return View(vm);
                }

            }
            return View(vm);
        }


    }
    [HttpGet("/admin/departamentos/eliminar/{id}")]
    public async Task<IActionResult> EliminarAsync(int id)
    {
         httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        var token = User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Realiza una solicitud DELETE a la API
        var response = await httpClient.DeleteAsync($"/api/Departamentos/{id}");

        if (response.IsSuccessStatusCode)
        {
            // La actividad se eliminó exitosamente
            return RedirectToAction("Index"); // Redirige a la página principal o a la lista de actividades
        }
        else
        {
            // Manejo del error, por ejemplo, mostrar un mensaje de error en la vista
            ModelState.AddModelError(string.Empty, "Error al eliminar la actividad");
            return View(); // Muestra la vista actual con el mensaje de error
        }
    }
}
