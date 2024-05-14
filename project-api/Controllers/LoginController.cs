using Microsoft.AspNetCore.Http;
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
            var dep = departamentosRepository.Get(login.Email);
            if (dep != null)
            {
                
                bool ver = Verifier.VerifyPassword(login.Password, dep.Password);
                if (ver)
                {
                    JwtTokenGenerator jwttoken=new JwtTokenGenerator(_configuration);
                    var token=jwttoken.GetToken(dep);
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
