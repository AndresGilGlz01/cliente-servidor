using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using project_api.Models.Dtos;
using project_api.Models.Entities;
using project_api.Repositories;
using project_api.Validators;

namespace project_api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin,Usuario")]
[ApiController]
public class ActividadesController(ActividadesRepository repository, ActividadValidator validator) : ControllerBase
{
    private ActividadesRepository actividadesRepository = repository;
    private readonly ActividadValidator actividadesValidator = validator;

    [HttpGet("{id}")]
    public IActionResult GetByDepartamento(int id)
    {
        var datos = actividadesRepository.GetActividades(id);
        return Ok(datos);
    }

    [HttpGet("/api/actividad/{id}")]
    public IActionResult GetAct(int id)
    {
        var datos = actividadesRepository.GetById(id);

        if (datos == null)  return BadRequest();

        var dto = new ActividadesDto()
        {
            Id = datos.Id,
            Titulo = datos.Titulo,
            Estado = datos.Estado,
            FechaCreacion = datos.FechaCreacion,
            FechaActualizacion = datos.FechaActualizacion,
            FechaRealizacion = datos.FechaRealizacion != null ? datos.FechaRealizacion.Value.ToDateTime(new TimeOnly()) : null,
            IdDepartamento = datos.IdDepartamento,
            Descripcion = datos.Descripcion,
            Departamento = datos.IdDepartamentoNavigation.Nombre,
        };

        return Ok(dto);
    }

    [HttpPost("/api/borrador")]
    public IActionResult Borrador(ActividadesDto dto)
    {
        if (dto == null) return BadRequest();

        DateOnly? fecha = null;
        
        if (dto.FechaRealizacion != null)
            fecha = DateOnly.FromDateTime(dto.FechaRealizacion.Value.Date);
        else
            fecha = DateOnly.FromDateTime(DateTime.Today);

        var currentDateTime = DateTime.Now;

        if (dto.FechaCreacion == DateTime.MinValue)
            dto.FechaCreacion = currentDateTime;

        if (dto.FechaActualizacion == DateTime.MinValue)
            dto.FechaActualizacion = currentDateTime;

        if (dto.FechaCreacion > currentDateTime)
            dto.FechaCreacion = currentDateTime;
        
        var actividad = new Actividades()
        {
            Id = 0,
            Estado = 0,
            Titulo = dto.Titulo,
            FechaCreacion = dto.FechaCreacion.Value,
            FechaActualizacion = dto.FechaActualizacion.Value,
            IdDepartamento = dto.IdDepartamento,
            Descripcion = dto.Descripcion,
            FechaRealizacion = fecha
        };

        actividadesRepository.Insert(actividad);

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{actividad.Id}.png");
        var bytes = Convert.FromBase64String(dto.Imagen);
        System.IO.File.WriteAllBytes(path, bytes);
        
        return Ok();
    }

    [HttpPost]
    public IActionResult Post(ActividadesDto? dto)
    {
        try
        {


            if (dto != null)
            {

                var results = actividadesValidator.Validate(dto);
                if (results.IsValid)
                {
                    DateOnly? fecha = null;
                    if (dto.FechaRealizacion != null)
                    {
                        fecha = System.DateOnly.FromDateTime(dto.FechaRealizacion.Value.Date);
                    }
                    else
                    {
                        fecha = System.DateOnly.FromDateTime(DateTime.Today);
                    }
                    DateTime now = DateTime.Now;


                    if (dto.FechaCreacion == DateTime.MinValue)
                    {
                        dto.FechaCreacion = now;
                    }
                    if (dto.FechaActualizacion == DateTime.MinValue)
                    {
                        dto.FechaActualizacion = now;
                    }

                    // Verificar si la fecha de creación es mayor que la fecha actual
                    if (dto.FechaCreacion > now)
                    {
                        dto.FechaCreacion = now;
                    }
                    if(dto.FechaRealizacion > now)
                    {
                        fecha= System.DateOnly.FromDateTime(now);
                    }
                    Actividades act = new Actividades()
                    {
                        Id = 0,
                        Estado = 1,
                        Titulo = dto.Titulo,
                        FechaCreacion = (DateTime)dto.FechaCreacion,
                        FechaActualizacion = (DateTime)dto.FechaActualizacion,
                        IdDepartamento = dto.IdDepartamento,
                        Descripcion = dto.Descripcion,
                        FechaRealizacion = fecha

                    };


                    actividadesRepository.Insert(act);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{act.Id}.png");
                    var bytes = Convert.FromBase64String(dto.Imagen);
                    System.IO.File.WriteAllBytes(path, bytes);
                    return Ok();

                    // call a method in other image controller



                }
                return BadRequest(results.Errors.Select(x => x.ErrorMessage));
            }
        }
        catch
        {

            return BadRequest();
        }
        return BadRequest();
    }

    [HttpPut]
    public IActionResult Put(ActividadesDto dto)
    {
        var vali = actividadesValidator.Validate(dto);
        if (vali.IsValid)
        {
            var act = actividadesRepository.GetById(dto.Id);
            if (act == null || act.Estado == 2)
            {
                return NotFound();
            }
            else
            {
                DateOnly? fecha = null;
                if (dto.FechaRealizacion != null)
                {
                    fecha = System.DateOnly.FromDateTime(dto.FechaRealizacion.Value.Date);
                }
                else
                {
                    fecha = System.DateOnly.FromDateTime(DateTime.Today);
                }
                DateTime now = DateTime.Now;

                if (dto.FechaRealizacion > now)
                {
                    fecha = System.DateOnly.FromDateTime(now);
                }
                act.Titulo = dto.Titulo;
                act.Estado = 1;
                act.FechaActualizacion = DateTime.UtcNow;
                act.Descripcion = dto.Descripcion;
                act.IdDepartamento = dto.IdDepartamento;
                act.FechaRealizacion = fecha;

                actividadesRepository.Update(act);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{dto.Id}.png");
                var bytes = Convert.FromBase64String(dto.Imagen);
                System.IO.File.WriteAllBytes(path, bytes);
                return Ok();

            }
        }
        return BadRequest(vali.Errors.Select(x => x.ErrorMessage));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var act = actividadesRepository.GetById(id);
        if (act == null || act.Estado == 2)
        {
            return NotFound();
        }
        act.Estado = 2;
        act.FechaActualizacion = DateTime.UtcNow;
        actividadesRepository.Update(act);
        return Ok();
    }
}
