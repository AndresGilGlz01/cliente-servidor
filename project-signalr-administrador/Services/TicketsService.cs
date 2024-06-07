using Microsoft.AspNetCore.SignalR.Client;

namespace project_signalr_administrador.Services;

public class TicketsService
{
    HubConnection hubConnection;

    public TicketsService()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://api.signalr.labsystec.net/ticketshub")
            .Build();

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        };
    }
}
