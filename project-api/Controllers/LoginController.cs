﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_api.Helpers;
using project_api.Models.Dtos;
using project_api.Repositories;

namespace project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DepartamentosRepository departamentosRepository;
        private readonly IConfiguration _configuration;
        public LoginController(DepartamentosRepository repo, IConfiguration configuration)
        {
            departamentosRepository = repo;
            _configuration = configuration;

        }
        [HttpPost]
        public IActionResult Login(LoginDto login)
        {
            var dep = departamentosRepository.Get(login.UserName);
            string role;
            if (dep != null)
            {
                
                bool ver = Verifier.VerifyPassword(login.Password, dep.Password);
                if (ver)
                {
                    if(dep.Nombre=="Direccion General")
                    {
                        role = "Admin";
                    }
                    else
                    {
                        role = "Usuario";
                    }
                    JwtTokenGenerator jwttoken=new JwtTokenGenerator(_configuration);
                    var token=jwttoken.GetToken(dep,role);
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Credenciales incorrectas");
                }
            }
            return BadRequest();
        }
    }
}
