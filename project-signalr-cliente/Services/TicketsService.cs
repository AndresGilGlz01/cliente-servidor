using Microsoft.AspNetCore.SignalR.Client;
using project_signalr_cliente.Models.Entities;

namespace project_signalr_cliente.Services;

public class TicketsService
{
    HubConnection hubConnection;
    public event EventHandler<string> ElfokinEvento;
    public TicketsService()
    {
        hubConnection = new HubConnectionBuilder()
            //.WithUrl("https://api.signalr.labsystec.net/ticketshub")
            .WithAutomaticReconnect()
            .WithUrl("https://localhost:7129/ticketshub")
            .Build();
        
        
        hubConnection.On<string>("TurnoNuevo", x =>
        {
            
            ElfokinEvento?.Invoke(this,x);
            //ListaTurnos.Add(x);
        });

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        };
        _= IniciarConeccion();
    }
    public async Task SolicitarTurno()
    {
        await hubConnection.InvokeAsync("SolicitarTurno");     
    }
    async Task IniciarConeccion()
    {
        await hubConnection.StartAsync();
    }
}
