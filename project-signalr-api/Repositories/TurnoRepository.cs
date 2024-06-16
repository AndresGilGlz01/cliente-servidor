using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class TurnoRepository : Repository<Turno>
{
    public TurnoRepository(TicketsContext context) : base(context) { }

    public async Task<Turno?> GetByFolio(string folio) => await Context.Turno.FirstOrDefaultAsync(x => x.Folio == folio);

    public async Task<Turno?> GetByCaja(int id)
    {
        var caja = await Context.Caja
            .Include(c => c.IdTurnoActualNavigation)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (caja is null) return null;

        return caja.IdTurnoActualNavigation;
    }
}
