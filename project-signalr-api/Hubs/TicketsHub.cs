using Microsoft.AspNetCore.SignalR;

using project_signalr_api.Converters;
using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;

namespace project_signalr_api.Hubs;

public class TicketsHub(TurnoRepository turnoRepository, CajaRepository cajaRepository) : Hub
{
    private readonly TurnoRepository turnoRepository = turnoRepository;
    private readonly CajaRepository cajaRepository = cajaRepository;

    public async Task RequestTurno()
    {
        var turno = new Turno
        {
            Folio = Guid.NewGuid().ToString().Substring(0, 8),
            Estado = "Pendiente",
            Fecha = DateTime.UtcNow,
        };

        await turnoRepository.Insert(turno);

        var response = turno.ToResponse();

        await Clients.All.SendAsync("NuevoTurno", response);
    }

    public async Task UpdateCajaState(UpdateCajaRequest request)
    {
        var caja = await cajaRepository.GetById(request.IdCaja);

        caja.IdAdministradorActual = request.IdAdministrador;

        var entity = await cajaRepository.Update(caja);
        var response = entity.ToResponse();

        await Clients.All.SendAsync("CajaActualizada", response);
    }

    public async void SolicitarTurno()
    {
        //fucking codigo

        //solo quiero probar xd
        Random r = new Random();
        string turno = $"A{r.Next(1000, 9004)}";
        await Clients.Caller.SendAsync("TurnoNuevo", turno);
    }
}
