using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CompanyPMO_.NET.Hubs
{
    public sealed class TimelineHub : Hub<ITimelineClient>
    {
        public async Task SendTimelineEvent(TimelineShowcaseDto timelineEvent)
        {
            await Clients.All.ReceiveTimelineEvent(timelineEvent);
        }
    }
}
