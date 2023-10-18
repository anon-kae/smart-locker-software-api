using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess
{
    public class NotificationDA : INotificationDA
    {
        private readonly IBaseRepository baseRepository;

        public NotificationDA(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public List<NotificationDto> GetAllNotificationByAccountId(int page, int perPage, int accountId)
        {
            page = (page - 1) * perPage;
            string sql = $@"SELECT NOTI.[NotiId]
                              ,NOTI.[Description]
                              ,NOTI.[ReadStatus]
                              ,NOTI.[Status]
                              ,NOTI.[CreateDate]
                              ,NOTI.[EventId]
                              ,NOTI.[AccountId]
	                          ,ET.[EventName]
                          FROM [dbo].[NOTIFICATIONS] NOTI
                          INNER JOIN EVENT_TYPES ET ON ET.[EventId] = NOTI.[EventId] AND ET.Status = 'A'
                          WHERE NOTI.Status = 'A' AND NOTI.AccountId = {accountId}
                          ORDER BY NOTI.[CreateDate] DESC
                          OFFSET {page} ROWS FETCH NEXT {perPage} ROWS ONLY ";
            List<NotificationDto> notificationResult = baseRepository.QueryString<NotificationDto>(sql).ToList();
            return notificationResult;
        }
    }
}
