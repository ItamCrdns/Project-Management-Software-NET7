using CompanyPMO_.NET.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CompanyPMO_.NET.Hubs
{
    [Authorize(Policy = "EmployeesAllowed")]
    public sealed class NotificationHub : Hub<INotificationClient>
    {
    }
}
