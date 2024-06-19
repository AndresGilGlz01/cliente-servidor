using signalrCliente.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddSingleton<TicketService>();
builder.Services.AddHttpClient("server", options =>
{
    options.BaseAddress = new Uri("https://api.signalr.labsystec.net/");
});
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
