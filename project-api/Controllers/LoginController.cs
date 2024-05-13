using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_api.Helpers;
using project_api.Repositories;

namespace project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DepartamentosRepository departamentosRepository;
        public LoginController(DepartamentosRepository repo)
        {
            departamentosRepository = repo;
        }
        [HttpPost]
        public IActionResult Login(string email,string password)
        {
            var pass=Encrypter.HashPassword(password);
            var dep = departamentosRepository.Get(email);
            if(dep != null)
            {
               bool ver=Verifier.VerifyPassword(pass,dep.Password);
                if(ver)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}
