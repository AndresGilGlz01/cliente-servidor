using project_api.Models.Entities;

namespace project_api.Repositories
{
    public class ActividadesRepository : Repository<Actividades>
    {
        private readonly ItesrcneActividadesContext context;
        public ActividadesRepository(ItesrcneActividadesContext ctx) : base(ctx)
        {
            context = ctx;
        }

    }
}
