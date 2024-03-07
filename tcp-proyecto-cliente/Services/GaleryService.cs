using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

using tcp_proyecto_cliente.Models.DTOs;

namespace tcp_proyecto_cliente.Services;

public class GaleryService
{
    public event EventHandler? OnSendMessage;
    public event EventHandler? OnConnectServer;
    public event EventHandler? OnReceiveMessage;
    public event EventHandler? OnDisconnectServer;
    public event EventHandler? OnDeniedServer;
    public event EventHandler? OnUnexpected;

    private readonly TcpClient _client = new();

    public string Username { get; set; } = null!;

    public GaleryService()
    {
        Username = Environment.UserName;
    }

    /// <summary>
    /// Initialize the connection to the server
    /// </summary>
    /// <param name="ipAddress">Server Ip Address</param>
    /// <param name="port">Server Port</param>
    public void Connect(IPAddress ipAddress, int port)
    {
        try
        {
            var socket = new IPEndPoint(ipAddress, port);

            Listen();
            _client.Connect(socket);

            var request = new ConnectDto
            {
                Name = Username,
                Message = "**HELLO",
            };
        }
        catch (Exception) { }
    }

    /// <summary>
    /// Method to disconnect from the server
    /// </summary>
    public void Disconect()
    {
        try
        {
            var request = new ConnectDto
            {
                Name = Username,
                Message = "**BYE",
            };

            SendMessage(request);

            _client.Close();
        }
        catch (Exception) { }
    }

    /// <summary>
    /// Method to send a message to the server
    /// </summary>
    /// <param name="request">**HELLO to connect, **BYE to disconnect</param>
    public void SendMessage(ConnectDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Message)) return;

        var json = JsonSerializer.Serialize(request);
        var buffer = Encoding.UTF8.GetBytes(json);
        var stream = _client.GetStream();

        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }

    /// <summary>
    /// Method to send a picture to the server
    /// </summary>
    /// <param name="picture">Object with Autor and base 64 image</param>
    public void SendMessage(PictureDto picture)
    {
        if (string.IsNullOrWhiteSpace(picture.Image)) return;

        if (string.IsNullOrWhiteSpace(picture.Autor)) return;

        var json = JsonSerializer.Serialize(picture);
        var buffer = Encoding.UTF8.GetBytes(json);
        var stream = _client.GetStream();

        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
    }

    /// <summary>
    /// Method to listen the server
    /// </summary>
    public void Listen()
    {
        var thread = new Thread(() =>
        {
            while (_client.Connected)
            {
                if (_client.Available > 0)
                {
                    var stream = _client.GetStream();
                    var buffer = new byte[_client.Available];
                    stream.Read(buffer, 0, buffer.Length);

                    var request = JsonSerializer.Deserialize<ConnectDto>(Encoding.UTF8.GetString(buffer));

                    if (request is null) continue;
                    
                    if (request.Message == "**OK")
                    {
                        OnConnectServer?.Invoke(this, EventArgs.Empty);
                    }
                    else if (request.Message == "**DENIED")
                    {
                        OnDeniedServer?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        OnUnexpected?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        });

        thread.IsBackground = true;
        thread.Start();
    }
}
