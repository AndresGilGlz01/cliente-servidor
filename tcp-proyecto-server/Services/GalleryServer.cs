using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using tcp_proyecto_server.Models.DTOs;

namespace tcp_proyecto_server.Services
{
    public class GalleryServer
    {
        TcpListener server = null!;
        List<TcpClient> clients = new List<TcpClient?>();

        public event EventHandler<PictureDto> ImagenRecibido;
        public event EventHandler<MensajeDto> MensajeRecibido;

        public void Iniciar(IPAddress ip)
        {
            server = new(new IPEndPoint(ip, 9630));  
            server.Start();
            new Thread(Escuchar) { IsBackground = true }.Start();

        }

        private void Escuchar(object? obj)
        {
            while (server.Server.IsBound)
            {
                var tcpClient = server.AcceptTcpClient();
                clients.Add(tcpClient); 

                Thread t = new(async () =>
                {
                    await RecibirMensajes(tcpClient);
                });
                t.IsBackground = true;
                t.Start();
            }
        }

        private async Task RecibirMensajes(TcpClient cliente)
        {
            var ns = cliente.GetStream();
            byte[] lengthBuffer = new byte[4]; // Assuming length is a 32-bit integer

            while (cliente.Connected)
            {
                await ns.ReadAsync(lengthBuffer, 0, 4);
                int messageLength = BitConverter.ToInt32(lengthBuffer);

                byte[] messageBuffer = new byte[messageLength];
                int bytesRead = 0;
                while (bytesRead < messageLength)
                {
                    bytesRead += await ns.ReadAsync(messageBuffer, bytesRead, messageLength - bytesRead);
                }

                string json = Encoding.UTF8.GetString(messageBuffer);

                var mensaje1 = JsonSerializer.Deserialize<PictureDto>(json);
                var mensaje = JsonSerializer.Deserialize<MensajeDto>(json);

                if (mensaje1?.Image != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ImagenRecibido?.Invoke(this, mensaje1);
                    });
                }

                if (mensaje?.Message != null || mensaje?.Name != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MensajeRecibido?.Invoke(this, mensaje);
                    });
                }
            }
        }
    }
}
