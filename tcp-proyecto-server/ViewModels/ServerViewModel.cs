using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using tcp_proyecto_server.Models.DTOs;
using tcp_proyecto_server.Services;

namespace tcp_proyecto_server.ViewModels
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public GalleryServer server { get; set; } = new();
        public ObservableCollection<string> Usuarios { get; set; } = new();
        public ObservableCollection<MensajeDto> Mensajes { get; set; } = new();
        public ObservableCollection<PictureDto> ImagenesUsuarios { get; set; } = new();

        int IndentificadorImg = 0;
        public string IP { get; set; } = "0.0.0.0";

        public ICommand IniciarServerCommand { get; set; }

        public ServerViewModel()
        {
            var direcciones = Dns.GetHostAddresses(Dns.GetHostName());
            if (direcciones != null)
            {
                IP = direcciones.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(x => x.ToString()).FirstOrDefault()!;
            }

            IniciarServerCommand = new RelayCommand(IniciarServer);
            server.MensajeRecibido += Server_MensajeRecibido;
            server.ImagenRecibido += Server_ImagenRecibido;
            IniciarServer();
        }

        private void IniciarServer()
        {
            IPAddress.TryParse(IP, out IPAddress ip);
            
            server.Iniciar(ip);
        }

        private void Server_ImagenRecibido(object? sender, PictureDto e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (e.Image != null && e.Autor != null)
                {
                    if (e.Subject == "Add")
                    {
                        string base64Image = e.Image;
                        byte[] imageBytes = Convert.FromBase64String(base64Image);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            System.Drawing.Image imagen = System.Drawing.Image.FromStream(ms);
                            string archivo = $"imagen{IndentificadorImg}.png";
                            string carpetaUsuarios = "UsersImages";
                            string rutaCarpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, carpetaUsuarios);

                            if (!Directory.Exists(rutaCarpeta))
                            {
                                Directory.CreateDirectory(rutaCarpeta);
                            }

                            string rutaCompleta = Path.Combine(rutaCarpeta, archivo);

                            imagen.Save(rutaCompleta, System.Drawing.Imaging.ImageFormat.Png);

                            BitmapImage bitmapimage = new();
                            bitmapimage = new BitmapImage(new Uri(rutaCompleta));
                            e.img = bitmapimage;
                        }

                        IndentificadorImg++;
                        ImagenesUsuarios.Add(e);
                    }
                    else if (e.Subject == "Remove")
                    {
                        var imagen = ImagenesUsuarios.FirstOrDefault(x =>
                            x.Autor == e.Autor
                            && x.Date == e.Date);

                        if (imagen != null)
                        {
                            ImagenesUsuarios.Remove(imagen);
                        }
                    }
                }
            });
        }

        private void Server_MensajeRecibido(object? sender, MensajeDto e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (e.Message == "**HELLO")
                {
                    e.Message = $"{e.Name} se ha conectado";
                    Usuarios.Add(e.Name);
                }
                else if (e.Message == "**BYE")
                {
                    e.Message = $"{e.Name} se ha desconectado";
                    Usuarios.Remove(e.Name);

                    var elementosAEliminar = new List<PictureDto>();

                    foreach (var elemento in ImagenesUsuarios)
                    {
                        if (elemento.Autor == e.Name)
                        {
                            elementosAEliminar.Add(elemento);
                        }
                    }

                    foreach (var item in elementosAEliminar)
                    {
                        ImagenesUsuarios.Remove(item);
                    }

                }
                Mensajes.Add(e);
            });
        }


    }
}
