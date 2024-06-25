namespace project_signalr_api.Models.DTOs.Response;

public class EstadisticaResponse
{
    public int CantidadEspera { get; set; }
    public int CantidadAtendiendo { get; set; }
    public int CantidadAtendidos { get; set; }
    public int? CajaMasFrecuente { get; set; }
    public int? CajaMenosFrecuente { get; set; }
    public string VolumenDeUsuarios { get; set; } = "";
    public string TiempoPromedioDeEspera { get; set; } = "";

}
