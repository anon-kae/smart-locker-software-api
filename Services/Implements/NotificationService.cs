using AutoMapper;
using smartlocker.software.api.Models;
using smartlocker.software.api.Services.DataAccess.Interface;
using smartlocker.software.api.Services.Interfaces;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using System.Linq;
using System.Collections.Generic;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using System;

namespace smartlocker.software.api.Services.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly IBaseRepository baseRepository;
        private readonly INotificationDA notificationDA;
        private readonly ISqlDA sql;
        private readonly IMapper mapper;
        public NotificationService(IBaseRepository baseRepository, ISqlDA sql , IMapper mapper , INotificationDA notificationDA)
        {
            this.baseRepository = baseRepository;
            this.sql = sql;
            this.mapper = mapper;
            this.notificationDA = notificationDA;
        }

        public string AddNotification(NotificationDto noti)
        {
            string queryString = sql.InsertNotification(noti);
            var result = baseRepository.ExecuteString<int>(queryString);

            if (result == 1)
                return "Insert notification sucessful";
            else
                return "Insert notification unsucessful";
        }

        public ResponseHeader GetAllEventType()
        {
            using var context = new SmartLockerContext();
            var eventTypeList = context.EventTypes.Where(c => c.Status == "A").ToList();
            List<EventTypeDto> eventTypeDtoList = mapper.Map<List<EventTypes>, List<EventTypeDto>>(eventTypeList);
            return new ResponseHeader("S", "Get All EventType", eventTypeDtoList);
        }


        public ResponseHeader GetAllNotificationByAccountId(int page , int perPage , int accountId) 
        {
            List<NotificationDto> notificationResult = notificationDA.GetAllNotificationByAccountId(page, perPage, accountId);
            if(notificationResult == null)
            {
                throw new ArgumentException("accountId is invalid.");
            }
            return new ResponseHeader("S", "Get All Notification By AccountId Successful", notificationResult,page,perPage, notificationResult.Count);

        }
    }
}