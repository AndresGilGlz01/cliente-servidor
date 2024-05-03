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

        public ActividadesController(ActividadesRepository repository)
        {
            _repository = repository;
        }
        ActividadValidator validator = new();
        [HttpGet]
        public IActionResult Get()
        {
            ActividadesDto datos = new();
            datos = (ActividadesDto)_repository.GetActividades();
            return Ok(datos);

        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           var datos = _repository.GetById(id);
            if(datos != null)
            {

                return Ok(datos);
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult Post(ActividadesDto dto)
        {
            var results=validator.Validate(dto);
            if(results.IsValid)
            {

                Actividades act = new Actividades()
                {
                    Id = 0,
                    Estado = 0,
                    Titulo = dto.Titulo,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    IdDepartamento = dto.IdDepartamento,
                    Descripcion = dto.Descripcion,
                };
                _repository.Insert(act);
                return Ok(act);
            }
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));
        }
        
    }
}
