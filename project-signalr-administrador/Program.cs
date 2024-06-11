var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpClient("server", options =>
{
    options.BaseAddress = new Uri("https://api.signalr.labsystec.net/");
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapDefaultControllerRoute();

app.Run();
