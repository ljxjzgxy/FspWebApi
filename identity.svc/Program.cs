using System.Reflection;
using fsp.lib;
using fsp.lib.Appsettings;
using fsp.lib.crypto;
using fsp.lib.DependencyInjection.Individual;
using fsp.lib.DependencyInjection.UseInjection;
using fsp.lib.HttpClient;
using fsp.lib.Jwt;
using fsp.lib.Middleware;
using fsp.lib.Postgresql;
using fsp.lib.Postgresql.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
await builder.AddEssentialCustomServices(assemblyName);

builder.Services.Configure<ServiceUriInternal>(builder.Configuration.GetSection(nameof(ServiceUriInternal)));
builder.Services.Configure<PostgresqlSettings>(builder.Configuration.GetSection(nameof(PostgresqlSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddTransient<IDbPostgresql, DbPostgresql>();
builder.Services.AddSingleton<ICrypto, Crypto>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUsersService, UsersService>(); 


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsTest())
{
    app.UseSwagger();
    app.UseCustomSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();

//app.UseAuthorization();

app.MapControllers();

app.UserCustomHealthCheck();

app.Run();
