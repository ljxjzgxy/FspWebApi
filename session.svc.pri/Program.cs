using System.Reflection;
using fsp.lib;
using fsp.lib.Appsettings;
using fsp.lib.DependencyInjection;
using fsp.lib.Middleware;
using fsp.lib.Session;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var assemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
builder.Services.AddCustomSwagger(assemblyName);

var redisSettings = new RedisSettings();
builder.Configuration.Bind(nameof(RedisSettings), redisSettings);
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(  
        new ConfigurationOptions
        {
            EndPoints = { redisSettings.ConnectionString! },
        })
    );
builder.Services.AddSingleton<ISessionService,SessionService>();
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
