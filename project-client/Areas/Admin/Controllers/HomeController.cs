﻿using Microsoft.AspNetCore.Authorization;
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

            if (vm.IdDepartamento == 0||vm.IdDepartamento==null)
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


            var actdto = new AddActDto()
            {
                Titulo = vm.Titulo,
                FechaActualizacion = vm.FechaActualizacion,
                Descripcion = vm.Descripcion,
                FechaCreacion = vm.FechaCreacion,
                FechaRealizacion = vm.FechaRealizacion,
                IdDepartamento = vm.IdDepartamento??0,
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

    [HttpGet("Admin/home/editar/{id}")]
    public async Task<IActionResult> Editar(int id)
    {


        GetActividadViewModel act = new();
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");


        var r = await httpClient.GetAsync($"/api/actividades/GetAct/{id}");
        if (r.IsSuccessStatusCode)
        {
            var con = await r.Content.ReadAsStringAsync();
            act.Actividad = JsonConvert.DeserializeObject<Actividad>(con); ;

            return View(act);
        }



        return View(null);
    }
    [HttpPost]
    public async Task<IActionResult> Editar(GetActividadViewModel act)
    {
        
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        // Convertir el objeto act a JSON
        var converter = new ConverterToBase64(webHostEnvironment);

        // Suponiendo que tienes una propiedad 'Imagen' en tu ViewModel que contiene la imagen como un byte array
        // Aquí debes reemplazar 'vm.Imagen' con la propiedad real que contiene la imagen en tu ViewModel
        var imagenBase64 = "";
        if (act.Actividad.Archivo != null)
        {

            var ruta = converter.SaveFile(act.Actividad.Archivo);
            imagenBase64 = converter.ImageToBase64(ruta);
        }
        if (act.Actividad.IdDepartamento == 0|| act.Actividad.IdDepartamento == null)
        {
            var userid = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            act.Actividad.IdDepartamento = int.Parse(userid);
        }
        var acti=new AddActDto()
        {
            Id=act.Actividad.Id,
            Descripcion=act.Actividad.Descripcion,
            Titulo=act.Actividad.Titulo,
            IdDepartamento=act.Actividad.IdDepartamento??0,
            FechaCreacion=act.Actividad.FechaCreacion,
            FechaRealizacion=act.Actividad.FechaRealizacion,
            Imagen=imagenBase64

        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(acti), Encoding.UTF8, "application/json");
        
        // Hacer la solicitud PUT a la API
        var response = await httpClient.PutAsync("/api/actividades", jsonContent);

        if (response.IsSuccessStatusCode)
        {

            // Puedes retornar la vista adecuada si es necesario
            return RedirectToAction("Index");
        }
        else
        {

            var r = await httpClient.GetAsync($"/api/actividades/{act.Actividad.Id}");
            if (r.IsSuccessStatusCode)
            {
                var con = await r.Content.ReadAsStringAsync();
                act.Actividad = JsonConvert.DeserializeObject<Actividad>(con); ;

                return View(act);
            }
        }
        return View(act);
        
    }


    [HttpGet("admin/home/eliminar/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        httpClient.BaseAddress = new Uri("https://sga.api.labsystec.net/");

        // Realiza una solicitud DELETE a la API
        var response = await httpClient.DeleteAsync($"/api/Actividades/{id}");

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
