using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text.Json;

using udp_wpf_server.Models;
using udp_wpf_server.Models.DTOs;
using udp_wpf_server.Services;

namespace udp_wpf_server.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _ipAddress = "0.0.0.0";

    [ObservableProperty]
    private ObservableCollection<Taller> _talleres = [];

    private List<Taller> talleres = [];

    private readonly WorkshopService _server = new();

    public MainViewModel()
    {
        var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());

        _server.OnRegistred += Server_OnRegistred;

        IpAddress = ipAddresses
            .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .Select(x => x.ToString()).FirstOrDefault() ?? "0.0.0.0";

        LoadData();
        Update();
    }

    private void Server_OnRegistred(object? sender, InscribeDto e)
    {
        if (e.Taller == "Ninguno")
        {
            foreach (var taller in talleres)
            {
                var alumno = taller.Alumnos.FirstOrDefault(x => x.Nombre == e.Nombre);

                if (alumno != null) taller.Alumnos.Remove(alumno);
            }
        }
        else
        {
            var taller = talleres.FirstOrDefault(x => x.Nombre == e.Taller);

            taller?.Alumnos.Add(new Alumno { Nombre = e.Nombre });
        }

        Save();
        Update();
    }

    private void Save()
    {
        var file = File.Create("talleres.json");
        
        JsonSerializer.Serialize(file, talleres);
        
        file.Close();
    }

    public void Update()
    {
        Talleres.Clear();

        foreach (var t in talleres)
        {
            Talleres.Add(t);
        }
    }

    public void LoadData()
    {
        if (File.Exists("talleres.json"))
        {
            var file = File.OpenRead("talleres.json");

            talleres = JsonSerializer.Deserialize<List<Taller>>(file) 
                ?? [ new Taller() { Nombre = "Canto", Alumnos = [] }, 
                    new Taller() { Nombre = "Canto", Alumnos = [] } ];

            file.Close();
        }
        else
        {
            talleres = [ 
                new Taller() { Nombre = "Canto", Alumnos = [] }, 
                new Taller() { Nombre = "Baile", Alumnos = [] } ];
        }
    }
}
