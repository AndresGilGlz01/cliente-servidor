using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using udp_wpf_client.Models.DTOs;

namespace udp_wpf_client.Services;

public class WorkshopService
{
    public string IpAddress { get; set; } = "0.0.0.0";

    private readonly UdpClient client = new();

    public void SendRequest(InscribeDto inscribe)
    {
        var ipe = new IPEndPoint(IPAddress.Parse(IpAddress), 5001);

        var json = JsonSerializer.Serialize(inscribe);

        var buffer = Encoding.UTF8.GetBytes(json);

        client.Send(buffer, buffer.Length, ipe);
    }
}
