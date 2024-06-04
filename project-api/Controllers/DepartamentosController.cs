using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_api.Helpers;
using project_api.Models.Dtos;
using project_api.Models.Entities;
using project_api.Repositories;
using project_api.Validators;

namespace project_api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin,Usuario")]
[ApiController]
public class DepartamentosController : ControllerBase
{
    private readonly DepartamentosRepository _departamentosRepository;
    private readonly DepartamentosValidator _validator;
    private readonly ActividadesRepository _actividadesRepository;
    public DepartamentosController(DepartamentosRepository repodep, DepartamentosValidator validations, ActividadesRepository actividadesRepository)
    {
        _departamentosRepository = repodep;
        _validator = validations;
        _actividadesRepository = actividadesRepository;
    }
    [HttpGet]
    public IActionResult Get()
    {
        var datos = _departamentosRepository.GetDeparamentos();
        return Ok(datos);
    }
    [HttpGet("/api/departamento/{id}")]   
    public IActionResult GetDepa(int id)
    {
        var datos = _departamentosRepository.Get(id);
        if (datos != null)
        {
            var dto = new DepartamentosDto()
            {
                Id = datos.Id,
                Nombre = datos.Nombre,
                Username = datos.Username,
                Password = datos.Password,
                DepartamentoSuperior = datos.IdSuperior != null ? _departamentosRepository.GetById(datos.IdSuperior.Value)?.Nombre : null,
                IdSuperior = datos.IdSuperior
            };
            return Ok(dto);
        }
        return NotFound();
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var datos = _departamentosRepository.GetSub(id).Select(x=>new DepartamentosDto()
        {
            Id=x.Id,
            Nombre=x.Nombre,
            Username = x.Username,
            IdSuperior=x.IdSuperior,
            DepartamentoSuperior=x.IdSuperiorNavigation?.Nombre,
            Password = x.Password,
        });
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
                return Ok();
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
        if (dep == null)
        {
            return NotFound();

        }
        var acts = _actividadesRepository.GetAct(dep.Id).ToList();
        if (acts.Any())
        {

            foreach (var act in acts)
            {
                _actividadesRepository.Delete(act);
            }
        }
        var depas = _departamentosRepository.GetSubs(dep.Id).ToList();
        if (depas.Any())
        {

            foreach (var item in depas)
            {
                item.IdSuperior = dep.IdSuperior;
                _departamentosRepository.Update(item);
            }
        }
        _departamentosRepository.Delete(dep);
        return NoContent();
    }
}
