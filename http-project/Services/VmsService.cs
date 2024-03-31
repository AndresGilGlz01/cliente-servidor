using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;

namespace http_project.Services;

public class VmsService
{
    private readonly HttpListener _listener = new();

    public event EventHandler<Object>? OnMessageReceived;

    public VmsService()
    {
        _listener.Prefixes.Add("http://*:6300/vms/");
    }

    public void Start()
    {
        if (_listener.IsListening) return;

        _listener.Start();

        var thread = new Thread(Listen)
        {
            IsBackground = true
        };

        thread.Start();
    }

    private void Listen()
    {
        //while (true)
        //{
        //    var context = _listener.GetContext();
        //    var page = File.ReadAllText("assets/index.html");
        //    var buffpagina = Encoding.UTF8.GetBytes(page);

        //    if (context.Request.Url != null)
        //    {
        //        if (context.Request.Url.LocalPath == "/notas/")
        //        {
        //            context.Response.ContentLength64 = buffpagina.Length;
        //            context.Response.OutputStream.Write(buffpagina, 0, buffpagina.Length);
        //            context.Response.StatusCode = 200;
        //            context.Response.Close();
        //        }
        //        else if (context.Request.HttpMethod == "POST" && context.Request.Url.LocalPath == "/notas/crear")
        //        {
        //            byte[] buffer = new byte[context.Request.ContentLength64];
        //            context.Request.InputStream.Read(buffer, 0, buffer.Length);
        //            string datos = Encoding.UTF8.GetString(buffer);


        //            var diccionario = HttpUtility.ParseQueryString(datos);


        //            Notas nota = new()
        //            {
        //                Titulo = diccionario["titulo"] ?? "",
        //                Contenido = diccionario["contenido"] ?? "",
        //                X = double.Parse(diccionario["x"] ?? "0"),
        //                Y = double.Parse(diccionario["y"] ?? "0"),
        //                Remitente = Dns.GetHostEntry(context.Request.RemoteEndPoint.Address).HostName,

        //            };
        //            Application.Current.Dispatcher.Invoke(() =>
        //            {
        //                NotaRecibida?.Invoke(this, nota);
        //            }

        //            );
        //            context.Response.StatusCode = 200;
        //            context.Response.Close();
        //        }
        //        else
        //        {
        //            context.Response.StatusCode = 404;
        //            context.Response.Close();

        //        }
        //    }
        //}

    }

    public void Stop()
    {
        _listener.Stop();
    }
}
