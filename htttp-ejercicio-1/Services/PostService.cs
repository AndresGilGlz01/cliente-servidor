using System.IO;
using System.Net;
using System.Text;

namespace htttp_ejercicio_1.Services;

public class PostService
{
    private readonly HttpListener _httpListener;

    public PostService()
    {
        _httpListener = new HttpListener();

        _httpListener.Prefixes.Add("http:/*:12345/notes");
    }

    public void Start()
    {
        if (_httpListener.IsListening) return;

        _httpListener.Start();

        Console.WriteLine("Listening...");

        var thread = new Thread(HandleRequests);

        thread.IsBackground = true;
        thread.Start();
    }

    private void HandleRequests()
    {
        while (true)
        {
            var context = _httpListener.GetContext();
            var request = context.Request;
            var response = context.Response;

            var page = File.ReadAllText("Assets/Index.html");
            var buffer = Encoding.UTF8.GetBytes(page);

            if (request.Url!.LocalPath == "/notes/")
            {
                response.ContentLength64 = buffer.Length;
                response.StatusCode = 200;
                
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.Close();
            }

            //if (request.HttpMethod == "POST")
            //{
            //    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            //    {
            //        var data = reader.ReadToEnd();
            //        Console.WriteLine(data);
            //    }
            //}

            response.Close();
        }
    }

    public void Stop()
    {
        if (!_httpListener.IsListening) return;

        _httpListener.Stop();
        _httpListener.Close();
    }
}
