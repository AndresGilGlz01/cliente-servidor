using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;

using udp_wpf_server.Models.DTOs;
using System.Windows;

namespace udp_wpf_server.Services;

public class WorkshopService
{
    public event EventHandler<InscribeDto>? OnRegistred;

    public WorkshopService()
    {
        var thread = new Thread(new ThreadStart(Listen))
        {
            IsBackground = true
        };

        thread.Start();
    }

    private void Listen()
    {
        UdpClient server = new(5001);

        while (true)
        {
            var remoto = new IPEndPoint(IPAddress.Any, 5001);

            var buffer = server.Receive(ref remoto);

            var message = JsonSerializer.Deserialize<InscribeDto>(Encoding.UTF8.GetString(buffer));

            if (message != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OnRegistred?.Invoke(this, message);
                });
            }
        }
    }
}
