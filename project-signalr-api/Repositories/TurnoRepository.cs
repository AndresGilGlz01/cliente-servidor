using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class TurnoRepository : Repository<Turno>
{
    public TurnoRepository(TicketsContext context) : base(context) { }
}
