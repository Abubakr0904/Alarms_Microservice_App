using Alarm.Hangfire.Models;
using Alarm.Hangfire.RabbitMQ;
using Hangfire;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Alarm.Hangfire.RabbitMq;

public sealed class RabbitListener : IHostedService
{
    private IConnection _connection = null!;
    private IModel _channel = null!;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Register();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Unregister();

        return Task.CompletedTask;
    }

    private void Register()
    {
        var rabbitMQSettings = RabbitMQConfigurationReader.Configuration
            .GetSection(AppConstants.RabbitMQConstants.ConnectionSettings).Get<RabbitMQSettings>();

        var factory = GetConnectionFactory(rabbitMQSettings!);

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            var createAlarmCommand = JsonSerializer.Deserialize<CreateAlarmCommand>(message)!;
            var delay = createAlarmCommand.AlarmDateInUtc - DateTime.UtcNow;
            BackgroundJob.Schedule(() => CreateAlarm(createAlarmCommand), delay);
            Console.WriteLine(message);
        };

        _channel.BasicConsume(queue: AppConstants.RabbitMQConstants.HangfireQueueName, autoAck: true, consumer: consumer);
    }

    private void Unregister()
    {
        if (_channel != null)
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }

    private Task CreateAlarm(CreateAlarmCommand command)
    {
        var response = GetResponseAsBytes(command);
        using var channel = _connection.CreateModel();
        var properties = channel.CreateBasicProperties();

        channel.BasicPublish(
          exchange: AppConstants.RabbitMQConstants.WriteToDBExchange,
          routingKey: AppConstants.RabbitMQConstants.WriteToDBRoutingKey,
          mandatory: false,
          basicProperties: properties,
          body: response);

        Console.WriteLine("Response published");

        return Task.CompletedTask;
    }

    private static byte[] GetResponseAsBytes(CreateAlarmCommand command)
    {
        var responseModel = new AlarmCreatedResponse(command);
        var responseMessage = JsonSerializer.Serialize(responseModel);
        var responseBytes = Encoding.UTF8.GetBytes(responseMessage);

        return responseBytes;
    }

    private static ConnectionFactory GetConnectionFactory(RabbitMQSettings rabbitMQSettings)
    {
        return new ConnectionFactory
        {
            HostName = rabbitMQSettings.HostName,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.UserName,
            Password = rabbitMQSettings.Password,
            VirtualHost = rabbitMQSettings.VirtualHost,
            RequestedHeartbeat = TimeSpan.FromSeconds(rabbitMQSettings.RequestedHeartbeatInSeconds),
            AutomaticRecoveryEnabled = rabbitMQSettings.AutomaticRecoveryEnabled,
            TopologyRecoveryEnabled = rabbitMQSettings.TopologyRecoveryEnabled
        };
    }
}
