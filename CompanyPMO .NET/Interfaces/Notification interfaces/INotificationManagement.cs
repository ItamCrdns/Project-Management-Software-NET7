using CompanyPMO_.NET.Common;

namespace CompanyPMO_.NET.Interfaces.Notification_interfaces
{
    public interface INotificationManagement
    {
        Task<OperationResult> SendNotificationsBulk(string notificationName, string notificationContent, int senderId, int[] receiverIds); 
    }
}
