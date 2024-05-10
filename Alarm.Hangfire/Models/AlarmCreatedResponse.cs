namespace Alarm.Hangfire.Models;

public sealed class AlarmCreatedResponse(CreateAlarmCommand command)
{
    public string AlarmMessage { get; } = command.AlarmMessage;

    public int WorkerId { get; } = command.WorkerId;
}
