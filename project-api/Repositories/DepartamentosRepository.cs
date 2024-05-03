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
    }
}
