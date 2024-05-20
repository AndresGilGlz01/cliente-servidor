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
        public IEnumerable<ActividadesDto> GetActividades(int idepa)
        {
            // Obtener los IDs de todos los departamentos subordinados
            var departamentosSubordinadosIds = GetAllSubordinateDepartmentIds(idepa);

            // Incluir el ID del departamento principal
            departamentosSubordinadosIds.Add(idepa);

            // Obtener las actividades de estos departamentos
            return context.Actividades
                .Where(a => departamentosSubordinadosIds.Contains(a.IdDepartamento))
                .OrderBy(x => x.FechaCreacion)
                .Include(x => x.IdDepartamentoNavigation)
                .Select(x => new ActividadesDto
                {
                    Descripcion = x.Descripcion,
                    Id = x.Id,
                    FechaCreacion = x.FechaCreacion,
                    FechaActualizacion = x.FechaActualizacion,
                    Titulo = x.Titulo,
                    Estado = x.Estado,
                    IdDepartamento = x.IdDepartamento,
                    Departamento = x.IdDepartamentoNavigation.Nombre
                })
                .OrderByDescending(x => x.FechaActualizacion)
                .ToList();
        }


        private List<int> GetAllSubordinateDepartmentIds(int parentId)
        {
            var subordinateIds = context.Departamentos
                .Where(d => d.IdSuperior == parentId)
                .Select(d => d.Id)
                .ToList();

            var allSubordinateIds = new List<int>(subordinateIds);

            foreach (var id in subordinateIds)
            {
                allSubordinateIds.AddRange(GetAllSubordinateDepartmentIds(id));
            }

            return allSubordinateIds;
        }
        public Actividades? GetById(int id)
        {
            return context.Actividades.Include(x => x.IdDepartamentoNavigation).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable< Actividades> GetAct(int id)
        {
            return context.Actividades.Where(x => x.IdDepartamento == id);
        }
    }
}
