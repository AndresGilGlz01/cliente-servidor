﻿using Microsoft.AspNetCore.Mvc;

using project_signalr_administrador.Models.ViewModel.Home;

using System.IdentityModel.Tokens.Jwt;

namespace project_signalr_administrador.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
    readonly HttpClient httpClient = httpClientFactory.CreateClient("server");

    public IActionResult Index()
    {
        var token = HttpContext.Session.GetString("token");

        if (string.IsNullOrEmpty(token)) return RedirectToAction(nameof(Login));

        var idusuario = GetClaimValue(token, "nameid");
        var usuario = GetClaimValue(token, "unique_name");

        HttpContext.Session.SetString("idusuario", idusuario);
        HttpContext.Session.SetString("usuario", usuario);
        
        return View();
    }

    public IActionResult Historial()
    {
        var token = HttpContext.Session.GetString("token");

        return string.IsNullOrEmpty(token) ? RedirectToAction(nameof(Login)) : View();
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginRequest)
    {
        var request = new Models.DTOs.Request.LoginRequest
        {
            Username = loginRequest.NombreUsuario,
            Password = loginRequest.Contraseña
        };

        var response = await httpClient.PostAsJsonAsync("api/login", request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, responseContent);

            return View(loginRequest);
        }

        var token = responseContent; 

        HttpContext.Session.SetString("token", token);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("token");
        HttpContext.Session.Remove("idusuario");
        HttpContext.Session.Remove("usuario");

        return RedirectToAction(nameof(Login));
    }

    public static string? GetClaimValue(string token, string claimType)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(token))
        {
            var jwtToken = handler.ReadJwtToken(token);

            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);

            return claim?.Value;
        }

        return null;
    }
}
