using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using project_api.Models.Dtos;
using project_api.Models.Entities;

namespace project_api.Repositories
{
    public class DepartamentosRepository : Repository<Departamentos>
    {
        private readonly ItesrcneActividadesContext context;
        public DepartamentosRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
            context = ctx;
        }

        public IEnumerable<DepartamentosDto> GetDeparamentos()
        {
            return context.Departamentos.OrderBy(x=>x.Nombre).Select(d => new DepartamentosDto
            {
                Id = d.Id,
                IdSuperior = d.IdSuperior,
                Password = d.Password,
                Nombre = d.Nombre,
                Username = d.Username,
            });
        }

        public Departamentos? GetById(int id)
        {
            return context.Departamentos.Find(id);
        }
    }
}
