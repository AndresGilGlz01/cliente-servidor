namespace project_signalr_api.Models.DTOs.Response;

public class CajaResponse
{
    public int Id { get; set; }
    public int NumeroCaja { get; set; }
    public string? Administrador { get; set; }
    public int? IdTurnoActual { get; set; }
    public int? IdAdministradorActual { get; set; }
}
