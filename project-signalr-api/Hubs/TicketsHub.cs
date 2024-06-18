using Microsoft.AspNetCore.SignalR;

using project_signalr_api.Converters;
using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;

namespace project_signalr_api.Hubs;

public class TicketsHub(TurnoRepository turnoRepository,
    HistorialRepository historialRepository,
    CajaRepository cajaRepository) : Hub
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
        var cajas = await cajaRepository.GetAll();

        foreach (var c in cajas)
        {
            if (c.IdAdministradorActual == request.IdAdministrador)
            {
                c.IdAdministradorActual = null;
                await cajaRepository.Update(c);
            }
        }

        var caja = await cajaRepository.GetById(request.IdCaja);
        
        caja.IdAdministradorActual = request.IdAdministrador;

        var entity = await cajaRepository.Update(caja);
        var response = entity.ToResponse();

        await Clients.All.SendAsync("CajaActualizada", response);
    }

    public async Task UpdateTurnoState(int idCaja)
    {
        var caja = await cajaRepository.GetById(idCaja);

        if (caja?.IdTurnoActual != null)
        {
            var turno = await turnoRepository.GetById(caja.IdTurnoActual.Value);

            turno.Estado = "Atendido";

            await turnoRepository.Update(turno);
        }

        var siguienteTurno = turnoRepository.GetAll().Result
            .Where(t => t.Estado == "Pendiente")
            .OrderBy(t => t.Fecha)
            .FirstOrDefault();

        if (siguienteTurno != null)
        {
            caja.IdTurnoActual = siguienteTurno.Id;
            await cajaRepository.Update(caja);

            siguienteTurno.Estado = "Atendiendo";
            await turnoRepository.Update(siguienteTurno);
        }
        else
        {
            caja.IdTurnoActual = null;
        }

        var response = caja?.ToResponse();

        await Clients.All.SendAsync("TurnosActualizados", response);
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
