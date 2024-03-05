using System.Net.Sockets;
using System.Net;
using System.Text;

var endpoint = new IPEndPoint(IPAddress.Any, 9630);

var server = new UdpClient(endpoint)
{
    EnableBroadcast = true
};

while (true)
{
    var buffer = server.Receive(ref endpoint);

    var datos = Encoding.UTF8.GetString(buffer);

    Console.WriteLine($"[{DateTime.Now.ToString()}] - Mensaje recibido: {datos}");
}
