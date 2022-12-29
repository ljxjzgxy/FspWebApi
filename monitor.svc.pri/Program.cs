using System.Reflection;
using fsp.lib;
using fsp.lib.DependencyInjection;
using fsp.lib.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var assemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
builder.Services.AddCustomSwagger(assemblyName);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p => {
        p.WithOrigins(new string[] { "http://192.168.1.171","http://127.0.0.1:5173" , "http://localhost:5173" })
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Logging.AddMongoDbLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsTest())
{
    app.UseSwagger();
    app.UseCustomSwaggerUI();
}

app.UseCors();
//app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
