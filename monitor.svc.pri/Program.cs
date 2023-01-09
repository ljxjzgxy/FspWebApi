using System.Reflection;
using fsp.lib;
using fsp.lib.DependencyInjection;
using fsp.lib.DependencyInjection.Individual;
using fsp.lib.DependencyInjection.UseInjection;
using fsp.lib.Middleware;
using monitor.svc.pri;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var assemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
await builder.AddEssentialCustomServices(assemblyName);


builder.Services.AddCustomCorsOrigins(); 

builder.Services.AddHostedService<ServicesMonitorDaemon>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsTest())
{
    app.UseSwagger();
    app.UseCustomSwaggerUI();
}

app.UseCors(); 

app.UseMiddleware<LoggingMiddleware>(); 

app.MapControllers();

app.UserCustomHealthCheck();

app.Run();
