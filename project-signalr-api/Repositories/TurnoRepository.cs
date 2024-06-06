using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class TurnoRepository : Repository<Turno>
{
    public TurnoRepository(TicketsContext context) : base(context) { }

    public async Task<Turno?> GetByFolio(string folio) => await Context.Turno.FirstOrDefaultAsync(x => x.Folio == folio);
}
