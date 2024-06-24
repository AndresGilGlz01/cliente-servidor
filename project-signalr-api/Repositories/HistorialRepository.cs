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

    public int GetCajaMasFrecuenteHoy() 
    {
        var cajaMasFrecuente = Context.Caja
            .Include(caja => caja.Historial)
            .Where(caja => caja.Historial.Any(historial => historial.FechaAtencion.Date == DateTime.UtcNow.Date))
            .OrderByDescending(caja => caja.Historial.Count)
            .Select(caja => caja.Id)
            .FirstOrDefault();

        return cajaMasFrecuente;
    }

    public int GetCajaMenosFrecuenteHoy() 
    {
        var cajaMenosFrecuente = Context.Caja
            .Include(caja => caja.Historial)
            .Where(caja => caja.Historial.Any(historial => historial.FechaAtencion.Date == DateTime.UtcNow.Date))
            .OrderBy(caja => caja.Historial.Count)
            .Select(caja => caja.Id)
            .FirstOrDefault();

        return cajaMenosFrecuente;
    }
}