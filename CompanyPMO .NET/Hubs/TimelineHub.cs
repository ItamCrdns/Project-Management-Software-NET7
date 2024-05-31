using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CompanyPMO_.NET.Hubs
{
    [Authorize(Policy = "SupervisorOnly")]
    public sealed class TimelineHub : Hub<ITimelineClient>
    {
        public override async Task OnConnectedAsync()
        {
            var role = Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, role);

            await base.OnConnectedAsync();
        }
    }
}
