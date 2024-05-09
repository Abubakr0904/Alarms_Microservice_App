using Alarm.Api.RabbitMQ;
using Alarm.Api.UseCases.Alarms.Requests;

namespace Alarm.Api.UseCases.Alarms.Services;

public sealed class AlarmsService : IAlarmsService
{
    public Task CreateAlarm(SetAlarmRequest request)
    {
        RabbitMQMessagePublisher.PublishMessage(
            exchange: Constants.CreateAlarmConstants.CreateAlarmExchange,
            routingKey: Constants.CreateAlarmConstants.CreateAlarmRoutingKey,
            body: request,
            persistent: true);

        return Task.CompletedTask;
    }
}
