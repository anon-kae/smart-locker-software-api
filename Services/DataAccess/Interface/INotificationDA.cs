using smartlocker.software.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface INotificationDA 
    {
        List<NotificationDto> GetAllNotificationByAccountId(int page, int perPage, int accountId);
    }
}
