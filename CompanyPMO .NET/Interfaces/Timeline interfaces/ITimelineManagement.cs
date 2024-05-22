using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces.Timeline_interfaces
{
    public interface ITimelineManagement
    {
        Task<OperationResult> CreateTimelineEvent(TimelineDto timeline);
        Task<OperationResult> DeleteTimelineEvents(int[] timelineIds);
    }
}
