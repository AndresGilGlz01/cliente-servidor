using System.Net;
using System.Net.Sockets;
using System.Text;

var client = new TcpClient();

Console.Write("Escribe la IP del servidor [default 127.0.0.1]: ");
var entry = Console.ReadLine()!;

entry = string.IsNullOrEmpty(entry) ? "127.0.0.1" : entry;

var ipaddress = new IPEndPoint(IPAddress.Parse(entry), 9630);

Console.WriteLine($"Conectandose a {ipaddress}...");
client.Connect(ipaddress);

Console.WriteLine("Escribe [byebye] para salir");

string mensaje = string.Empty;

while (mensaje != "byebye")
{
    Console.Write("Escriba el mensaje: ");
    mensaje = Console.ReadLine()!;
    
    byte[] buffer = Encoding.UTF8.GetBytes(mensaje);
    var ns = client.GetStream();

    ns.Write(buffer, 0, buffer.Length);
}
