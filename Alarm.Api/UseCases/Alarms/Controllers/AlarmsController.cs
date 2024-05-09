using Alarm.Api.UseCases.Alarms.Requests;
using Alarm.Api.UseCases.Alarms.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alarm.Api.UseCases.Alarms.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class AlarmsController(IAlarmsService alarmsService) : ControllerBase
{
    private readonly IAlarmsService _alarmsService = alarmsService;

    /// <summary>
    /// Create new alarm to be written to the database by the specified worker at the specified time.
    /// </summary>
    /// <param name="request">A model representing the alarm to be created.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public Task<IActionResult> CreateAlarm([FromBody] SetAlarmRequest request)
    {
        _alarmsService.CreateAlarm(request);

        return Task.FromResult<IActionResult>(Ok());
    }
}
