using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_api.Helpers;
using project_api.Models.Dtos;
using project_api.Models.Entities;
using project_api.Repositories;
using project_api.Validators;
using System.Xml;

namespace project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly DepartamentosRepository _departamentosRepository;
        private readonly DepartamentosValidator _validator;
        public DepartamentosController(DepartamentosRepository repodep, DepartamentosValidator validations)
        {
            _departamentosRepository = repodep;
            _validator = validations;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var datos = _departamentosRepository.GetDeparamentos();
            return Ok(datos);
        }
       

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var datos = _departamentosRepository.GetSub(id);
            if (datos != null)
            {

                return Ok(datos);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Post(DepartamentosDto dto)
        {
            var results = _validator.Validate(dto);
            if (results.IsValid)
            {
                dto.Password=Encrypter.HashPassword(dto.Password);
                Departamentos deo = new Departamentos()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Username = dto.Username,
                    Password = dto.Password,
                    IdSuperior = dto.IdSuperior,
                };

                _departamentosRepository.Insert(deo);
                return Ok(deo);
            }
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        }
        
        [HttpPut]
        public IActionResult Put(DepartamentosDto dto)
        {
            var results= _validator.Validate(dto);
            if(results.IsValid)
            {
                var dep=_departamentosRepository.GetById(dto.Id);
                if(dep != null)
                {
                    var encrypter = new Encrypter();
                    dep.Username = dto.Username;
                    
                    
                    dep.Nombre = dto.Nombre;
                    dep.IdSuperior = dto.IdSuperior;
                    if (encrypter.IsPasswordChanged(dep.Password, dto.Password))
                    {
                        dto.Password = Encrypter.HashPassword(dto.Password);
                        dep.Password=dto.Password.ToLower();
                    }
                    _departamentosRepository.Update(dep);
                    return Ok(dep);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dep = _departamentosRepository.GetById(id);
            if(dep != null)
            {
                _departamentosRepository.Delete(dep);
                return Ok(dep);

            }
            return NotFound();
        }
    }
}
