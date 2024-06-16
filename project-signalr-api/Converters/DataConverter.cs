using project_signalr_api.Models.DTOs.Request;
using project_signalr_api.Models.DTOs.Response;
using project_signalr_api.Models.Entities;

namespace project_signalr_api.Converters;

public static class DataConverter
{
    #region ToResponses
    public static HistorialResponse ToResponse(this Historial entity) => new()
    {
        Id = entity.Id,
        IdCaja = entity.IdCaja,
        IdTurno = entity.IdTurno,
        Estado = entity.Estado,
        Caja = new HistorialResponse.CajaResponse
        {
            Numero = entity.IdCajaNavigation.NumeroCaja
        },
        Folio = new HistorialResponse.FolioResponse
        {
            Folio = entity.IdTurnoNavigation.Folio,
            Estado = entity.IdTurnoNavigation.Estado,
            Fecha = entity.IdTurnoNavigation.Fecha
        },
    };

    public static CajaResponse ToResponse(this Caja entity) => new()
    {
        Id = entity.Id,
        NumeroCaja = entity.NumeroCaja,
        Administrador = entity.IdAdministradorActualNavigation?.NombreUsuario,
        IdTurnoActual = entity.IdTurnoActual,
        IdAdministradorActual = entity.IdAdministradorActual,
    };

    public static TurnoResponse ToResponse(this Turno entity) => new()
    {
        Id = entity.Id,
        Folio = entity.Folio,
        Estado = entity.Estado,
        Fecha = entity.Fecha,
    };
    #endregion

    #region ToEntity
    public static Turno ToEntity(this CreateTurnoRequest request) => new()
    {
        Folio = request.Folio ?? string.Empty,
    };
    #endregion
}
