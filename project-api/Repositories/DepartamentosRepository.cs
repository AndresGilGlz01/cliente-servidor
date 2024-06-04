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
        public Departamentos? Get(string email)
        {
            return context.Departamentos.Where(x => x.Username == email).FirstOrDefault();
        }

        public IEnumerable<DepartamentosDto> GetDeparamentos()
        {
            return context.Departamentos.OrderBy(x => x.Nombre).Select(d => new DepartamentosDto
            {
                Id = d.Id,
                IdSuperior = d.IdSuperior,
                Password = d.Password,
                Nombre = d.Nombre,
                Username = d.Username,
            });
        }
        public IEnumerable<Departamentos> GetSub(int id)
        {
            return context.Departamentos.Where(x => x.IdSuperior == id || x.Username.Contains("equipo3") && x.Id!=id).Include(x=>x.IdSuperiorNavigation);
        }
        public Departamentos? GetById(int id)
        {
            return context.Departamentos.Include(x => x.IdSuperiorNavigation).FirstOrDefault(x => x.Id == id);
        }
        public IEnumerable<Departamentos> GetSubs(int id)
        {
            return context.Departamentos.Where(x => x.IdSuperior == id ).Include(x => x.IdSuperiorNavigation);
        }
    }
}
