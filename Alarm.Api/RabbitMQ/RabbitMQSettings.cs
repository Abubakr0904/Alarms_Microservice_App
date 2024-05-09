namespace Alarm.Api.RabbitMQ;

public sealed class RabbitMQSettings
{
    public string HostName { get; set; }

    public int Port { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string VirtualHost { get; set; }
    
    public int RequestedHeartbeatInSeconds { get; set; }
    
    public bool AutomaticRecoveryEnabled { get; set; }
    
    public bool TopologyRecoveryEnabled { get; set; }
}
