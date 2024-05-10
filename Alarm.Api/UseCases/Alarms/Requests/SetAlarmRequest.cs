namespace Alarm.Api.UseCases.Alarms.Requests;

public record SetAlarmRequest(string AlarmMessage, DateTime AlarmDateInUtc, int WorkerId);
