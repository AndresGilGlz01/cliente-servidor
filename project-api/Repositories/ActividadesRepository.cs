using Microsoft.EntityFrameworkCore;
using project_api.Models.Dtos;
using project_api.Models.Entities;

namespace project_api.Repositories
{
    public class ActividadesRepository :Repository<Actividades>
    {
        private readonly ItesrcneActividadesContext context;
        public ActividadesRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
            context = ctx;
        }
        public  IEnumerable<ActividadesDto> GetActividades()
        {
            return context.Actividades.OrderBy(x => x.FechaCreacion).Include(x => x.IdDepartamentoNavigation).Select(x => new ActividadesDto()
            {
                Descripcion = x.Descripcion,
                Id = x.Id,
                FechaCreacion = x.FechaCreacion,
                FechaActualizacion = x.FechaActualizacion,
                Titulo = x.Titulo,
                Estado = x.Estado,
                IdDepartamento = x.IdDepartamento,
                Departamento = x.IdDepartamentoNavigation.Nombre

            }) ; 
        }
        public Actividades? GetById(int id)
        {
            return context.Actividades.Find(id);
        }
    }
}
