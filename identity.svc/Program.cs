using System.Reflection;
using fsp.lib;
using fsp.lib.Appsettings;
using fsp.lib.crypto;
using fsp.lib.HttpClient;
using fsp.lib.Jwt;
using fsp.lib.Middleware;
using fsp.lib.Postgresql;
using fsp.lib.Postgresql.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var assemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
builder.Services.AddCustomSwagger(assemblyName);

builder.Services.Configure<ServiceUriInternal>(builder.Configuration.GetSection(nameof(ServiceUriInternal)));
builder.Services.Configure<PostgresqlSettings>(builder.Configuration.GetSection(nameof(PostgresqlSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddTransient<IDbPostgresql, DbPostgresql>();
builder.Services.AddSingleton<ICrypto, Crypto>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddSingleton<IApiHelper, ApiHelper>();
builder.Logging.AddMongoDbLogger();


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

app.Run();
