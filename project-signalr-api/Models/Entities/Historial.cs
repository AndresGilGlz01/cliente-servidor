using System;
using System.Collections.Generic;

namespace project_signalr_api.Models.Entities;

public partial class Historial
{
    public int Id { get; set; }

    public int IdCaja { get; set; }

    public int IdTurno { get; set; }

    public DateTime FechaAtencion { get; set; }

    public virtual Caja IdCajaNavigation { get; set; } = null!;

    public virtual Turno IdTurnoNavigation { get; set; } = null!;
}
