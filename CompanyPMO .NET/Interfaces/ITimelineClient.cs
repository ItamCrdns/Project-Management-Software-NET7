using CompanyPMO_.NET.Dto;

namespace CompanyPMO_.NET.Interfaces
{
    public interface ITimelineClient
    {
        Task ReceiveTimelineEvent(TimelineShowcaseDto timeliveEvent);
    }
}
