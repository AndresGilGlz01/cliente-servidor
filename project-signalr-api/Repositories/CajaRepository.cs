using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class CajaRepository : Repository<Caja>
{
    public CajaRepository(TicketsContext context) : base(context) { }

    public override Task<Caja?> GetById(int id) => Context.Set<Caja>()
        .Include(c => c.IdAdministradorActualNavigation)
        .FirstOrDefaultAsync(c => c.Id == id);

    public override async Task<IEnumerable<Caja>> GetAll() => await Context.Set<Caja>()
        .Include(c => c.IdAdministradorActualNavigation)
        .ToListAsync();

    public async Task<int?> GetLastCaja()
    {
        var caja = await Context.Caja.OrderByDescending(c => c.Id).FirstOrDefaultAsync();

        return caja?.NumeroCaja;
    }

    public async Task<bool> Exists(int id) => await Context.Caja.AnyAsync(c => c.Id == id);
}
