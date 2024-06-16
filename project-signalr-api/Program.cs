using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using project_signalr_api.Hubs;
using project_signalr_api.Models.Entities;
using project_signalr_api.Repositories;
using project_signalr_api.Validators;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<TicketsContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.Parse("10.11.7-mariadb"));
});
builder.Services.AddTransient<AdministradorRepository>();
builder.Services.AddTransient<HistorialRepository>();
builder.Services.AddTransient<CajaRepository>();
builder.Services.AddTransient<TurnoRepository>();
builder.Services.AddTransient<LoginRequestValidator>();
builder.Services.AddTransient<UpdateTurnoRequestValidator>();
builder.Services.AddTransient<UpdateCajaRequestValidator>();
builder.Services.AddTransient<CreateTurnoRequestValidator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromHours(5),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", app =>
    {
        app.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<TicketsHub>("/ticketshub");

app.UseCors("MyPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
