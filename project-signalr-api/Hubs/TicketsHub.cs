using Microsoft.AspNetCore.SignalR;

namespace project_signalr_api.Hubs;

public class TicketsHub : Hub
{
    public async void SolicitarTurno()
    {
        //fucking codigo

        //solo quiero probar xd
        Random r = new Random();
        string turno = $"A{r.Next(1000, 9004)}";
        await Clients.Caller.SendAsync("TurnoNuevo", turno);
    }
}
