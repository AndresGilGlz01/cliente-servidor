namespace udp_wpf_server.Models;

public class Taller
{
    public string Nombre { get; set; } = null!;
    public List<Alumno> Alumnos { get; set; } = [];
}
