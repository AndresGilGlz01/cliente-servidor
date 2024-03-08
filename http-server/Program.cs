using System.Net;
using System.Text;

var server = new HttpListener();

server.Prefixes.Add("http://localhost:10000/");

server.Start();

Console.WriteLine("Listening...");

while (true)
{
    var context = server.GetContext();

    var nombre = context.Request.QueryString["nombre"];

    var response = string.Empty;

    if (!string.IsNullOrEmpty(nombre))
    {
        response = $"<h1>Hello, {nombre}!</h1>";
    }
    else
    {
        response = "<h1>Hello, World!</h1>";
    }

    var buffer = Encoding.UTF8.GetBytes(response);

    context.Response.ContentLength64 = buffer.Length;
    context.Response.ContentType = "text/html";
    context.Response.StatusCode = 200;

    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

    context.Response.Close();
}
