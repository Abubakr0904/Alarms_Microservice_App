namespace Alarm.Api.UseCases.Alarms.Requests;

public record SetAlarmRequest(string Name, string Description, DateTime alarmDate, int workerId);
