using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class HistorialRepository(TicketsContext context) : Repository<Historial>(context) { }