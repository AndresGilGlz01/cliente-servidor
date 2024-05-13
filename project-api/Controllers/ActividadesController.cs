using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public IActionResult Get()
        {
            var datos = _repository.GetActividades();
            return Ok(datos);

        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var datos = _repository.GetById(id);
            if (datos != null)
            {

                return Ok(datos);
            }
            return BadRequest();

        }


        [HttpPost]
        public IActionResult Post(ActividadesDto? dto)
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

                    Actividades act = new Actividades()
                    {
                        Id = 0,
                        Estado = 0,
                        Titulo = dto.Titulo,
                        FechaCreacion = DateTime.UtcNow,
                        FechaActualizacion = DateTime.UtcNow,
                        IdDepartamento = dto.IdDepartamento,
                        Descripcion = dto.Descripcion,
                        FechaRealizacion = fecha

                    };

                    // Conversión manual de fechaRealizacion si no es nulo
                    _repository.Insert(act);
                    return Ok();


                }
                return BadRequest(results.Errors.Select(x => x.ErrorMessage));
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
                if (act == null || act.Estado == 1)
                {
                    return NotFound();
                }
                else
                {

                    act.Titulo = dto.Titulo;
                    act.Estado = dto.Estado;
                    act.FechaActualizacion = DateTime.UtcNow;
                    act.Descripcion = dto.Descripcion;
                    act.IdDepartamento = dto.IdDepartamento;

                    _repository.Update(act);
                    return Ok();

                }
            }
            return BadRequest(vali.Errors.Select(x => x.ErrorMessage));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var act = _repository.GetById(id);
            if (act == null || act.Estado == 1)
            {
                return NotFound();
            }
            act.Estado = 1;
            act.FechaActualizacion = DateTime.UtcNow;
            _repository.Update(act);
            return Ok();
        }

    }
}
