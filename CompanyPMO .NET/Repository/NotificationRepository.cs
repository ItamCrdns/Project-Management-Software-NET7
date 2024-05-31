using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Notification_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.SignalR;

namespace CompanyPMO_.NET.Repository
{
    public class NotificationRepository : INotificationManagement
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        public NotificationRepository(ApplicationDbContext context, IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<OperationResult> SendNotificationsBulk(string notificationName, string notificationContent, int senderId, int[] receiverIds)
        {
            var notifications = receiverIds.Select(receiverId => new Notification
            {
                Name = notificationName,
                Content = notificationContent,
                Created = DateTime.UtcNow,
                SenderId = senderId,
                ReceiverId = receiverId
            }).ToList();

            await _context.AddRangeAsync(notifications);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                foreach (int receiverId in receiverIds)
                {
                    var notification = new Notification
                    {
                        Name = notificationName,
                        Content = notificationContent,
                        Created = DateTime.UtcNow,
                        SenderId = senderId,
                        ReceiverId = receiverId
                    };

                    await _hubContext.Clients.User(receiverId.ToString()).ReceiveNotification(notification);
                }
            }

            return new OperationResult
            {
                Success = rowsAffected > 0,
                Message = rowsAffected > 0 ? "Notifications sent successfully" : "Failed to send notifications"
            };
        }
    }
}
