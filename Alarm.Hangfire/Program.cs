using Alarm.Hangfire;
using Alarm.Hangfire.RabbitMq;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString(AppConstants.HangfireConstants.DbConnection)));
builder.Services.AddHangfireServer();

builder.Services.AddHostedService<RabbitListener>();
RabbitMQConfigurationReader.Configuration = builder.Configuration;

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

app.Run();
