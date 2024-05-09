using System.Text.Json;
using System.Text;

namespace Alarm.Api.RabbitMQ;

public static class RabbitMQMessagePublisher
{
    public static void PublishMessage<T>(string exchange, string routingKey, T body, bool persistent = false, bool isMandatory = false)
    {
        using var channel = RabbitMQConnection.Instance.CreateChannel();
        var messageText = JsonSerializer.Serialize(body);
        var message = Encoding.UTF8.GetBytes(messageText);
        var properties = channel.CreateBasicProperties();
        properties.Persistent = persistent;

        channel.BasicPublish(exchange, routingKey, isMandatory, properties, message);
    }
}
