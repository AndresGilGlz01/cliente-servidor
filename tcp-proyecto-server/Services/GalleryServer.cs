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

        public void Iniciar()
        {
            server = new(new IPEndPoint(IPAddress.Any, 9630));  
            server.Start();
            new Thread(Escuchar) { IsBackground = true }.Start();

        }

        private void Escuchar(object? obj)
        {
            while (server.Server.IsBound)
            {
                var tcpClient = server.AcceptTcpClient();
                clients.Add(tcpClient); ////??

                Thread t = new(() =>
                {
                    RecibirMensajes(tcpClient);
                });
                t.IsBackground = true;
                t.Start();
            }
        }

        private void RecibirMensajes(TcpClient cliente)
        {
            while (cliente.Connected)
            {
                var ns = cliente.GetStream();

                while (cliente.Available == 0)
                {
                    Thread.Sleep(500);
                }

                byte[] buffer = new byte[cliente.Available];
                ns.Read(buffer, 0, buffer.Length);
                string json = Encoding.UTF8.GetString(buffer);



                var mensaje1 = JsonSerializer.Deserialize<PictureDto>(json);
                var mensaje = JsonSerializer.Deserialize<MensajeDto>(json);

                if (mensaje1.Image != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ImagenRecibido?.Invoke(this, mensaje1);
                    });
                }


                if (mensaje != null)
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
