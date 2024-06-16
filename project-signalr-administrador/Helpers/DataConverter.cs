using project_signalr_administrador.Models.DTOs.Response;
using project_signalr_administrador.Models.ViewModel.Home;

namespace project_signalr_administrador.Helpers;

public static class DataConverter
{
    public static IndexViewModel.TurnoModel ToModel(this TurnoResponse turnoResponse) => new()
    {
        Folio = turnoResponse.Folio,
        Fecha = turnoResponse.Fecha,
        Estado = turnoResponse.Estado,
    };

    public static HistorialViewModel.TurnoModel ToModel(this HistorialResponse historialResponse) => new()
    {
        Id = historialResponse.Id,
        IdCaja = historialResponse.IdCaja,
        IdTurno = historialResponse.IdTurno,
        FechaAtencion = historialResponse.FechaAtencion,
        Folio = new HistorialViewModel.FolioModel
        {
            Folio = historialResponse.Folio.Folio,
            Fecha = historialResponse.Folio.Fecha,
        },
        Caja = new HistorialViewModel.CajaModel
        {
            Numero = historialResponse.Caja.Numero,
        },
        Estado = historialResponse.Estado,
    };
}
