using Microsoft.EntityFrameworkCore;

using project_signalr_api.Models.Entities;

namespace project_signalr_api.Repositories;

public class AdministradorRepository(TicketsContext context) : Repository<Administrador>(context)
{
    public async Task<Administrador?> Login(string username, string password)
    {
        return await Context.Administrador.FirstOrDefaultAsync(a => a.NombreUsuario == username && a.Contraseña == password);
    }

    public override Task<Administrador?> GetById(int id) => throw new NotImplementedException();

    public override Task<IEnumerable<Administrador>> GetAll() => throw new NotImplementedException();

    public override Task<Administrador> Insert(Administrador entity) => throw new NotImplementedException();

    public override Task<Administrador> Update(Administrador entity) => throw new NotImplementedException();

    public override Task Delete(Administrador entity) => throw new NotImplementedException();
}
