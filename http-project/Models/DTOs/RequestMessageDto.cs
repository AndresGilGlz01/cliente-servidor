namespace http_project.Models.DTOs;

public class RequestMessageDto
{
    public string Message { get; set; } = null!;
    public Pictorama Pictorama { get; set; }
    public Status Status { get; set; }
}
