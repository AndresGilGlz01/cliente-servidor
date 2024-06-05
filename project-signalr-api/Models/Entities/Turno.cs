using System;
using System.Collections.Generic;

namespace project_signalr_api.Models.Entities;

public partial class Turno
{
    public int Id { get; set; }

    public string Folio { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public virtual ICollection<Caja> Caja { get; set; } = new List<Caja>();

    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
}
