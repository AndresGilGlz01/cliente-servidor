using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class HistorialRepository : Repository<Historial> 
{
    public HistorialRepository(TicketsContext context) : base(context) { }

    public async override Task<IEnumerable<Historial>> GetAll()
    {
        return await Context.Historial
            .Include(historial => historial.IdCajaNavigation)
            .Include(historial => historial.IdTurnoNavigation)
            .ToListAsync();
    }

    public async Task<int> GetCajaMasFrecuente() 
    {
        var cajaMasFrecuente = await Context.Historial
            .GroupBy(historial => historial.IdCaja)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .FirstOrDefaultAsync();

        return cajaMasFrecuente;
    }

    public async Task<int> GetCajaMenosFrecuente() 
    {
        var cajaMenosFrecuente = await Context.Historial
            .GroupBy(historial => historial.IdCaja)
            .OrderBy(group => group.Count())
            .Select(group => group.Key)
            .FirstOrDefaultAsync();

        return cajaMenosFrecuente;
    }
}