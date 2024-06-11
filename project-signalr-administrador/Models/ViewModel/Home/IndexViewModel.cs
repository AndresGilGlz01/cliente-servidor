namespace project_signalr_administrador.Models.ViewModel.Home;

public class IndexViewModel
{
    public TurnoModel? TurnoActual { get; set; }
    public IEnumerable<TurnoModel> Turnos { get; set; } = [];

    public class TurnoModel
    {
        public string Folio { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = null!;
    }
}
