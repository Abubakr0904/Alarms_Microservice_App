using Alarm.Api.UseCases.Alarms.Requests;

namespace Alarm.Api.UseCases.Alarms.Services;

public interface IAlarmsService
{
    Task CreateAlarm(SetAlarmRequest request);
}
