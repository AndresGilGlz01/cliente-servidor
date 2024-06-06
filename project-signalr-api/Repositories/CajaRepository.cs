using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class CajaRepository : Repository<Caja>
{
    public CajaRepository(TicketsContext context) : base(context) { }

    public async Task<int?> GetLastCaja()
    {
        var caja = await Context.Caja.OrderByDescending(c => c.Id).FirstOrDefaultAsync();

        return caja?.NumeroCaja;
    }
}
