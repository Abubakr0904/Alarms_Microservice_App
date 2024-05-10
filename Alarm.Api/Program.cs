using Alarm.Api.RabbitMQ;
using Alarm.Api.UseCases.Alarms.Services;

var builder = WebApplication.CreateBuilder(args);

RabbitMQConfigurationReader.Configuration = builder.Configuration;

builder.Services.AddTransient<IAlarmsService, AlarmsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
