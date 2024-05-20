using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITimeline
    {
        Task<OperationResult> CreateTimelineEvent(TimelineDto timeline);
        Task<OperationResult> DeleteTimelineEvents(int[] timelineIds);
        Task<DataCountPages<TimelineDto>> GetTimelineEvents(FilterParams filterParams);
        Task<DataCountPages<TimelineDto>> GetTimelineEventsByEmployee(int employeeId, FilterParams filterParams);
    }
}
