using Microsoft.AspNetCore.SignalR.Client;
using signalrCliente.Models;

namespace signalrCliente.Services
{
    public class TicketService
    {
        HubConnection hubConnection;
        public event EventHandler<Turno>? AlSolicitarTurno;
        public TicketService()
        {
            hubConnection = new HubConnectionBuilder()
                //.WithUrl("https://api.signalr.labsystec.net/ticketshub")
                .WithAutomaticReconnect()
                .WithUrl("https://localhost:7129/ticketshub")
                .Build();


            hubConnection.On<Turno>("NuevoTurno", x =>
            {

                AlSolicitarTurno?.Invoke(this, x);
            
            });

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };
            _ = IniciarConeccion();
        }
        public async Task SolicitarTurno()
        {
            await hubConnection.InvokeAsync("RequestTurno");
        }
        async Task IniciarConeccion()
        {
            await hubConnection.StartAsync();
        }
    }
}
