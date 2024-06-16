namespace project_signalr_administrador.Models.DTOs.Response;

public class HistorialResponse
{
    public int Id { get; set; }
    public int IdCaja { get; set; }
    public int IdTurno { get; set; }
    public DateTime FechaAtencion { get; set; }
    public CajaResponse Caja { get; set; } = null!;
    public FolioResponse Folio { get; set; } = null!;
    public string Estado { get; set; } = null!;

    public class CajaResponse
    {
        public int Numero { get; set; }
    }

    public class FolioResponse
    {
        public string Folio { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime Fecha { get; set; }
    }
}
