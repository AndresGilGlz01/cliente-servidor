using System;
using System.Collections.Generic;

namespace project_signalr_api.Models.Entities;

public partial class Caja
{
    public int Id { get; set; }

    public int NumeroCaja { get; set; }

    public int? IdTurnoActual { get; set; }

    public int? IdAdministradorActual { get; set; }

    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();

    public virtual Administrador? IdAdministradorActualNavigation { get; set; }

    public virtual Turno? IdTurnoActualNavigation { get; set; }
}
