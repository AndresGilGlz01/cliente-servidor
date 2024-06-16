namespace project_signalr_administrador.Models.ViewModel.Home;

public class HistorialViewModel
{
    public IEnumerable<TurnoModel> Turnos { get; set; } = [];

    public class TurnoModel
    {
        public int Id { get; set; }
        public int IdCaja { get; set; }
        public int IdTurno { get; set; }
        public DateTime FechaAtencion { get; set; }
        public FolioModel Folio { get; set; } = null!;
        public CajaModel Caja { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }

    public class FolioModel
    {
        public string Folio { get; set; } = null!;
        public DateTime Fecha { get; set; }
    }

    public class CajaModel
    {
        public int Numero { get; set; }
    }
}
