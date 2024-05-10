using RabbitMQ.Client;

namespace Alarm.Api.RabbitMQ;

public sealed class RabbitMQConnection
{
    private static readonly Lazy<RabbitMQConnection> instance = new(() => new RabbitMQConnection());
    private readonly IConnection _connection;

    private RabbitMQConnection()
    {
        var rabbitMQSettings = RabbitMQConfigurationReader.Configuration
            .GetSection("RabbitMQSettings:Default").Get<RabbitMQSettings>();

        var factory = GetConnectionFactory(rabbitMQSettings!);

        _connection = factory.CreateConnection();
    }

    public static RabbitMQConnection Instance
    {
        get
        {
            return instance.Value;
        }
    }

    public IModel CreateChannel()
    {
        return _connection.CreateModel();
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
