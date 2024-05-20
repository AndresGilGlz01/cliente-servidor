namespace project_client.Areas.Admin.Models;

public class AgregarDepartamentoViewModel
{
    public string Nombre { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int IdSuperior { get; set; }
    public IEnumerable<Departamentos> Departamentos { get; set; }

}
