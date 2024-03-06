using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

using tcp_wpf_server.Models.DTOs;

namespace tcp_wpf_server.Services;

public class ChatServer
{
    private TcpListener server = null!;

    private readonly List<TcpClient> clients = [];

    public event EventHandler<MessageDto>? OnReceivedMessage;

    public void Start()
    {
        server = new(new IPEndPoint(IPAddress.Any, 9630));
        server.Start();

        new Thread(Listen)
        {
            IsBackground = true
        }.Start();
    }

    void Listen()
    {
        while (server.Server.IsBound)
        {
            var client = server.AcceptTcpClient();

            clients.Add(client);

            var thread = new Thread(() =>
            {
                ReceiveMessage(client);
            })
            {
                IsBackground = true
            };

            thread.Start();
        }
    }

    void ReceiveMessage(TcpClient client)
    {
        while (client.Connected)
        {
            var stream = client.GetStream();

            while (client.Available == 0)
            {
                Thread.Sleep(500);
            }

            var buffer = new byte[client.Available];

            stream.Read(buffer, 0, buffer.Length);

            var json = Encoding.UTF8.GetString(buffer);

            var mensaje = JsonSerializer.Deserialize<MessageDto>(json);

            if (mensaje == null) continue;
            
            RelayMensaje(client, buffer);
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                OnReceivedMessage?.Invoke(this, mensaje);
            });
        }

        clients.Remove(client);
    }

    void RelayMensaje(TcpClient cliente, byte[] mensaje)
    {
        foreach (var item in clients)
        {
            if (item == cliente) continue;

            var stream = item.GetStream();
            
            stream.Write(mensaje, 0, mensaje.Length);
            stream.Flush();
        }
    }

    public void Stop()
    {
        try
        {
            if (server == null) return;

            server.Stop();

            foreach (var client in clients)
            {
                client.Close();
            }
        }
        catch (Exception) { }
    }
}
