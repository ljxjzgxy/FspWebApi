using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCustomSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}"); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseCustomSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
