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
}
