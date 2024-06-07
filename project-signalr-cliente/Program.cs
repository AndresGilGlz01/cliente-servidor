using project_signalr_cliente.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddSingleton<TicketsService>();
builder.Services.AddHttpClient("server", options =>
{
    options.BaseAddress = new Uri("https://api.signalr.labsystec.net/");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapDefaultControllerRoute();

app.Run();
