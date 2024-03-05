using System.Net;
using System.Net.Sockets;
using System.Text;

var client = new UdpClient // Definir cliente o servidor
{
    EnableBroadcast = true
};

Console.WriteLine("Iniciando cliente UDP... \n");
var endpoint = new IPEndPoint(IPAddress.Broadcast, 9630); // A donde enviar y solicitar datos

Console.WriteLine("Escribe [byebye] para salir.");

var mensaje = string.Empty;

while (mensaje != "byebye")
{
    Console.Write("Ingresa el mensaje que quieres enviar: ");
    mensaje = Console.ReadLine() ?? string.Empty;

    var buffer = Encoding.UTF8.GetBytes(mensaje);

    var bytesSended = client.Send(buffer, buffer.Length, endpoint);

    if (bytesSended > 0)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("¡Mensaje éxitosamente enviado!");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("¡Error al enviar el mensaje!");
        Console.ResetColor();
    }
}
