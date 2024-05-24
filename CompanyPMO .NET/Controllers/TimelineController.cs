using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPMO_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly ITimeline _timeline;
        public TimelineController(ITimeline timeline)
        {
            _timeline = timeline;
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("events/all")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<TimelineShowcaseDto>))]
        public async Task<IActionResult> GetAllTimelineEvents([FromQuery] FilterParams filterParams)
        {
            var timelineEvents = await _timeline.GetTimelineEvents(filterParams);

            return Ok(timelineEvents);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("employee/{employeeId}/events/all")]
        [ProducesResponseType(200, Type = typeof(DataCountPages<TimelineShowcaseDto>))]
        public async Task<IActionResult> GetEmployeeTimelineEvents(int employeeId, [FromQuery] FilterParams filterParams)
        {
            var timelineEvents = await _timeline.GetTimelineEventsByEmployee(employeeId, filterParams);

            return Ok(timelineEvents);
        }

        [Authorize(Policy = "SupervisorOnly")]
        [HttpGet("events/{timelineId}")]
        [ProducesResponseType(200, Type = typeof(TimelineDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTimelineEvent(int timelineId)
        {
            var timelineEvent = await _timeline.GetTimelineEvent(timelineId);

            if (timelineEvent == null)
            {
                return NotFound();
            }

            return Ok(timelineEvent);
        }
    }
}
