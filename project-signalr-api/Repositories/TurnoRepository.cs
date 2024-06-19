using Microsoft.EntityFrameworkCore;

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

    public async Task<Turno?> GetByUsuario(int id)
    {
        var usuario = await Context.Administrador
            .Include(u => u.Caja)
            .ThenInclude(c => c.IdTurnoActualNavigation)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (usuario is null) return null;

        var turno = usuario.Caja.FirstOrDefault()?.IdTurnoActualNavigation;

        return turno;
    }

    public async Task<int> GetCantidadTurnosEspera() => await Context.Turno
        .Where(x => x.Estado == "Pendiente")
        .CountAsync();

    public async Task<int> GetCantidadTurnosAtendiendo() => await Context.Turno
        .Where(x => x.Estado == "Atendiendo")
        .CountAsync();

    public async Task<int> GetCantidadTurnosAtendidos() => await Context.Turno
        .Where(x => x.Estado == "Atendido")
        .CountAsync();
}
