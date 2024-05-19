using Microsoft.AspNetCore.Mvc;

using project_api.Models.Dtos;
using project_api.Models.Entities;
using project_api.Repositories;
using project_api.Validators;

namespace project_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesController : ControllerBase
    {
        private ActividadesRepository _repository;
        private readonly ActividadValidator validator;

        public ActividadesController(ActividadesRepository repository, ActividadValidator validator)
        {
            _repository = repository;
            this.validator = validator;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var datos = _repository.GetActividades(id);
            return Ok(datos);

        }
        [HttpGet("GetAct/{id}")]
        public IActionResult GetAct(int id)
        {
            var datos = _repository.GetById(id);
            if (datos != null)
            {
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
            return BadRequest();

        }
        [HttpPost]
        public async Task<IActionResult> Post(ActividadesDto? dto)
        {
            try
            {

                
                if (dto != null)
                {

                    var results = validator.Validate(dto);
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
                        DateTime now = DateTime.UtcNow;
                        now=DateTime.Now;

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


                        _repository.Insert(act);

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
            var vali = validator.Validate(dto);
            if (vali.IsValid)
            {
                var act = _repository.GetById(dto.Id);
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
                    act.Titulo = dto.Titulo;
                    act.Estado = dto.Estado;
                    act.FechaActualizacion = DateTime.UtcNow;
                    act.Descripcion = dto.Descripcion;
                    act.IdDepartamento = dto.IdDepartamento;
                    act.FechaRealizacion = fecha;

                    _repository.Update(act);
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
            var act = _repository.GetById(id);
            if (act == null || act.Estado == 2)
            {
                return NotFound();
            }
            act.Estado = 2;
            act.FechaActualizacion = DateTime.UtcNow;
            _repository.Update(act);
            return Ok();
        }

    }
}
