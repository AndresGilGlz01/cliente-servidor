using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.Cookie.Name = "ASGcookie";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<HttpClient>();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapDefaultControllerRoute();
app.Run();
