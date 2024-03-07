using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using tcp_wpf_client.Models.DTOs;

namespace tcp_wpf_client.Services;

public class ChatService
{
    private TcpClient client = null!;

    public string Equipo { get; set; } = null!;

    public void Connect(IPAddress ipAddress)
    {
        try
        {
            var socket = new IPEndPoint(ipAddress, 9000);

            client = new TcpClient();
            client.Connect(socket);

            Equipo = Environment.UserName;

            var request = new MessageDto
            {
                Fecha = DateTime.Now,
                Mensaje = "**HELLO",
                Origen = Equipo
            };

            SendMessage(request);

            ReceiveMessage();
        }
        catch (Exception) { }
    }

    public void Disconect()
    {
        var request = new MessageDto
        {
            Fecha = DateTime.Now,
            Mensaje = "**BYE",
            Origen = Equipo
        };

        SendMessage(request);

        client.Close();
    }

    public event EventHandler<MessageDto>? OnReceiveMessage;

    private void ReceiveMessage()
    {
        new Thread(() =>
        {
            try
            {
                while (client.Connected)
                {
                    if (client.Available > 0)
                    {
                        var stream = client.GetStream();
                        
                        var buffer = new byte[client.Available];
                        
                        stream.Read(buffer, 0, buffer.Length);

                        var request = JsonSerializer.Deserialize<MessageDto>(Encoding.UTF8.GetString(buffer));

                        if (request != null) OnReceiveMessage?.Invoke(this, request);
                    }
                }
            }
            catch (Exception) { }
        })
        {
            IsBackground = true
        }.Start();
    }

    public void SendMessage(MessageDto message)
    {
        if (string.IsNullOrWhiteSpace(message.Mensaje)) return;

        var json = JsonSerializer.Serialize(message);

        var buffer = Encoding.UTF8.GetBytes(json);

        var stream = client.GetStream();

        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }
}
