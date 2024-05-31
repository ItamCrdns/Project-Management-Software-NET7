using CompanyPMO_.NET.Models;
using Task = System.Threading.Tasks.Task;

namespace CompanyPMO_.NET.Interfaces
{
    public interface INotificationClient
    {
        Task ReceiveNotification(Notification notification);
    }
}
