using System;
using System.Collections.Generic;

namespace project_signalr_api.Models.Entities;

public partial class Administrador
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Caja> Caja { get; set; } = new List<Caja>();
}
