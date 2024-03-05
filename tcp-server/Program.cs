using System.Net;
using System.Net.Sockets;
using System.Text;

var server = new TcpListener(IPAddress.Any, 9630);

server.Start();

var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());

var firstLocalIpv4 = ipAddresses
    .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
    .Select(x => x.ToString()).FirstOrDefault();

if (firstLocalIpv4 is not null)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Ready to listen from {firstLocalIpv4}:9630");
    Console.ResetColor();
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Any address found");
    Console.ResetColor();
    return;
}

while (true)
{
    var client = server.AcceptTcpClient();

    Console.WriteLine("New client connected " + client.Client.RemoteEndPoint?.ToString());

    var thread = new Thread(() =>
    {
        listenClient(client);
    })
    {
        IsBackground = true
    };

    thread.Start();
}


static void listenClient(TcpClient client)
{
    while (true)
    {
        if (client.Available > 0)
        {
            var stream = client.GetStream();

            var buffer = new byte[client.Available];
            
            stream.Read(buffer, 0, buffer.Length);

            var clientIp = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
            var message = Encoding.UTF8.GetString(buffer);
            
            Console.WriteLine($"[{DateTime.Now.ToShortDateString()}] - {clientIp}: {message}");
        }
    }
}
