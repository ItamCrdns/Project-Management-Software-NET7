using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITimeline
    {
        Task<DataCountPages<TimelineShowcaseDto>> GetTimelineEvents(FilterParams filterParams);
        Task<DataCountPages<TimelineShowcaseDto>> GetTimelineEventsByEmployee(int employeeId, FilterParams filterParams);
        Task<TimelineDto> GetTimelineEvent(int timelineId);
    }
}
