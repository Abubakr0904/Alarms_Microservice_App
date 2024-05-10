namespace Alarm.Hangfire.Models;

public record CreateAlarmCommand(string AlarmMessage, DateTime AlarmDateInUtc, int WorkerId);
