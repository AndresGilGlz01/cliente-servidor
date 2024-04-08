using http_project.Models.DTOs;

using Newtonsoft.Json;

using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;

namespace http_project.Services;

public class VmsService
{
    private readonly HttpListener _listener = new();

    public event EventHandler<IEnumerable<RequestMessageDto>>? OnMessageReceived;

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
        while (true)
        {
            var context = _listener.GetContext();

            var index = File.ReadAllText("assets/index.html");

            var buffer = Encoding.UTF8.GetBytes(index);

            if (context.Request.Url?.LocalPath == "/vms/")
            {

                if (context.Request.HttpMethod == "GET")
                {
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
                else if (context.Request.HttpMethod == "POST")
                {
                    using var reader = new StreamReader(context.Request.InputStream);

                    string json = reader.ReadToEnd();

                    var data = JsonConvert.DeserializeObject<IEnumerable<RequestMessageDto>>(json);

                    if (data != null)
                    {
                        OnMessageReceived?.Invoke(this, data);
                    }

                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}
