namespace project_client.Areas.Admin.Models;

public class EditarDepartamentoViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? DepartamentoSuperior { get; set; }
}
