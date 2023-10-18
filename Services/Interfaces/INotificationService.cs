using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Models.Output;

namespace smartlocker.software.api.Services.Interfaces
{
    public interface INotificationService
    {
        string AddNotification(NotificationDto noti);
        ResponseHeader GetAllEventType();
        ResponseHeader GetAllNotificationByAccountId(int page, int perPage, int accountId);
    }
}