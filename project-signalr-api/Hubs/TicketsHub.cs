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

    public async Task ToggleCaja(int idCaja)
    {
        var caja = await cajaRepository.GetById(idCaja);

        caja!.Abierta = !caja.Abierta;

        var entity = await cajaRepository.Update(caja);

        var response = entity.ToResponse();

        await Clients.All.SendAsync("CajaActualizada", response);
    }

    public async Task RequestTurno()
    {

        var isAvailedCajas = await cajaRepository.AnyAbierta();

        if (!isAvailedCajas) return;

        var turno = new Turno
        {
            Folio = Guid.NewGuid().ToString().Substring(0, 8),
            Estado = "Pendiente",
            Fecha = DateTime.UtcNow,
        };

        await turnoRepository.Insert(turno);

        var response = turno.ToResponse();
        await Clients.All.SendAsync("NuevoTurno", response);
        await Clients.Caller.SendAsync("NuevoTurnoT", response);
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

            var historial = new Historial
            {
                IdTurno = turno.Id,
                FechaAtencion = DateTime.UtcNow,
                IdCaja = caja.Id,
                Estado = "Atendido",
            };

            await historialRepository.Insert(historial);
        }

        var siguienteTurno = turnoRepository.GetAll().Result
            .Where(t => t.Estado == "Pendiente")
            .OrderBy(t => t.Fecha)
            .FirstOrDefault();

        if (siguienteTurno != null)
        {
            caja.IdTurnoActual = siguienteTurno.Id;
            caja.Abierta = true;
            await cajaRepository.Update(caja);
            await Clients.All.SendAsync("CajaActualizada", caja.ToResponse());

            siguienteTurno.Estado = "Atendiendo";
            await turnoRepository.Update(siguienteTurno);

            var historial = new Historial
            {
                IdTurno = siguienteTurno.Id,
                FechaAtencion = DateTime.UtcNow,
                IdCaja = caja.Id,
                Estado = "Atendiendo",
            };

            await historialRepository.Insert(historial);
        }
        else
        {
            caja.IdTurnoActual = null;
            caja.Abierta = true;
            await cajaRepository.Update(caja);
            await Clients.All.SendAsync("CajaActualizada", caja.ToResponse());
        }

        var response = caja?.ToResponse();

        await Clients.All.SendAsync("TurnosActualizados", response);
    }
}
