namespace project_signalr_administrador.Models.DTOs.Response;

public class TurnoResponse
{
    public int Id { get; set; }
    public string Folio { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public DateTime Fecha { get; set; }
}
