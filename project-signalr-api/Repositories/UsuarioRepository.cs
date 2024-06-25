using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;


public class UsuarioRepository : Repository<Administrador>
{
    public UsuarioRepository(TicketsContext context) : base(context) { }
}
