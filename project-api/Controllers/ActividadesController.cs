using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_api.Models.Dtos;
using project_api.Models.Entities;
using project_api.Repositories;
using project_api.Validators;
using System.Net.Http;

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
                    
                };
                return Ok(dto);
            }
            return BadRequest();

        }
        [HttpPost]
        public async Task<IActionResult> Post(ActividadesDto? dto)
        {
            var http=new HttpClient();
            http.BaseAddress = new Uri("https://sga.api.labsystec.net/");
            if (dto != null)
            {

                var results = validator.Validate(dto);
                if (results.IsValid)
                {
                    DateOnly? fecha = null;
                    if (dto.FechaRealizacion == null)
                    {
                        fecha = System.DateOnly.FromDateTime(dto.FechaRealizacion.Value.Date);
                    }
                    else
                    {
                        fecha = System.DateOnly.FromDateTime(DateTime.Today);
                    }
                    if (dto.FechaCreacion == DateTime.MinValue)
                    {
                        dto.FechaCreacion=DateTime.UtcNow;
                    }
                    if(dto.FechaActualizacion == DateTime.MinValue)
                    {
                        dto.FechaActualizacion=DateTime.UtcNow;
                    }
                    Actividades act = new Actividades()
                    {
                        Id = 0,
                        Estado = 1,
                        Titulo = dto.Titulo,
                        FechaCreacion = dto.FechaCreacion,
                        FechaActualizacion = dto.FechaActualizacion,
                        IdDepartamento = dto.IdDepartamento,
                        Descripcion = dto.Descripcion,
                        FechaRealizacion = fecha

                    };

                    
                    _repository.Insert(act);

                    var request = new HttpRequestMessage(HttpMethod.Post, 
                        $"https://sga.api.labsystec.net/{act.Id}");
                    var content = new StringContent(dto.Imagen, null, "application/json");
                    request.Content = content;
                    var response = await http.SendAsync(request);
                    response.EnsureSuccessStatusCode();
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
                if (act == null || act.Estado == 2)
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
            if (act == null || act.Estado == 2)
            {
                return NotFound();
            }
            act.Estado = 3;
            act.FechaActualizacion = DateTime.UtcNow;
            _repository.Update(act);
            return Ok();
        }

    }
}
