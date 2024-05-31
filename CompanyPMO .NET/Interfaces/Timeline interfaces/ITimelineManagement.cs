using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Timeline_interfaces
{
    public interface ITimelineManagement
    {
        Task<OperationResult> CreateTimelineEvent(TimelineDto timeline, string employeeTier);
        Task<OperationResult> CreateTimelineEventsBulk(List<TimelineDto> timelines, string employeeTier);
        Task<OperationResult> DeleteTimelineEvents(int[] timelineIds);
    }
}
