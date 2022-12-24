using System.Reflection;
using fsp.lib;
using fsp.lib.Appsettings;
using fsp.lib.DependencyInjection;
using fsp.lib.Session;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCustomSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}");

var redisSettings = new RedisSettings();
builder.Configuration.Bind(nameof(RedisSettings), redisSettings);
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(  
        new ConfigurationOptions
        {
            EndPoints = { redisSettings.ConnectionString! },
        })
    );
builder.Services.AddSingleton<ISessionService,SessionService>();    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsTest())
{
    app.UseSwagger();
    app.UseCustomSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
