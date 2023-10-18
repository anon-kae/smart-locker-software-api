using System;
using smartlocker.software.api.Models;
using smartlocker.software.api.Services.DataAccess.Interface;

namespace smartlocker.software.api.Services.DataAccess
{
    public class SqlDA : ISqlDA
    {
        public string InsertNotification(NotificationDto notification)
        {
            notification.Status = "A";
            notification.ReadStatus = "N";
            string queryString = $@"INSERT INTO NOTIFICATIONS 
                                           ([Description],[ReadStatus],[Status],[CreateDate],[EventId],[AccountId])
                                           VALUES
                                           ('{notification.Description}'
                                            ,'{notification.ReadStatus}'
                                            ,'{notification.Status}'
                                            ,GETDATE()
                                            ,{notification.EventId}
                                            ,{notification.AccountId})";
            return queryString;
        }

        public string InsertTransaction(string transactionNumber, decimal Amount , int? RateTimeId , int AccountId , int? LkRoomId)
        {
            string queryString = $@"INSERT INTO TRANSACTIONS 
                                           ([TransferId],[Amont],[CreateDate] , [RateTypeId] ,[AccountId] , [LkRoomId])
                                           VALUES
                                           ('{transactionNumber}',{Amount} , GETDATE() , {RateTimeId} , {AccountId} , {LkRoomId})";
            return queryString;
        }

        public string searchLocation(string keywords)
        {
            string queryString = $@"SELECT L.[LocateId]
                                          ,L.[LocateName]
                                          ,L.[Longtitude]
                                          ,L.[Latitude]
                                          ,L.[AddressId]
                                          ,L.[Status]
                                          ,L.[CreateDate]
                                          ,L.[UpdateDate]
                                          ,A.[Province]
                                          ,A.[PostalCode]
                                          ,A.[District]
                                    FROM [SmartLocker].[dbo].[LOCATIONS] L
                                    INNER JOIN ADDRESS A ON L.AddressId = A.AdressId AND A.Status = 'A'
                                    WHERE (L.LocateName Like '%{keywords}%'OR
                                           A.Province Like '%{keywords}%' OR
                                           A.District Like '%{keywords}%') AND L.Status = 'A' ";
            return queryString;
        }

        public string selectLocation()
        {
            string queryString = $@"SELECT * FROM LOCATIONS L 
                                    INNER JOIN ADDRESS A ON A.AddressId = L.AddressId AND A.Status = 'A'
                                    WHERE L.Status = 'A'";
            return queryString;
        }

        public string selectLocationById(int id)
        {
            
            string queryString = $@"SELECT L.[LocateId]
                                          ,L.[LocateName]
                                          ,L.[Longtitude]
                                          ,L.[Latitude]
                                          ,L.[AddressId]
                                          ,L.[Status]
                                          ,L.[CreateDate]
                                          ,L.[UpdateDate]
                                          ,A.[Province]
                                          ,A.[PostalCode]
                                          ,A.[District]
                                          ,A.[SubDistrict]
                                    FROM [SmartLocker].[dbo].[LOCATIONS] L
                                    INNER JOIN ADDRESS A ON L.AddressId = A.AddressId
                                    WHERE LocateId = {id} AND L.Status = 'A' AND A.Status = 'A'";
            return queryString;
        }
         public string selectLocationByAccountId(int page, int perPage,int id)
        {
            page = (page - 1) * perPage;
            string queryString = $@"SELECT L.[LocateId]
                                          ,L.[LocateName]
                                          ,L.[Longtitude]
                                          ,L.[Latitude]
                                          ,L.[AddressId]
                                          ,L.[Status]
                                          ,L.[CreateDate]
                                          ,L.[UpdateDate]
                                          ,A.[Province]
                                          ,A.[PostalCode]
                                          ,A.[District]
                                          ,A.[SubDistrict]
                                    FROM [SmartLocker].[dbo].[LOCATIONS] L
                                    INNER JOIN ADDRESS A ON L.AddressId = A.AddressId
                                    WHERE AccountId = {id} AND L.Status = 'A' AND A.Status = 'A'
                                    ORDER BY LocateId DESC
                                    OFFSET {page} ROWS FETCH NEXT {perPage} ROWS ONLY ";
            return queryString;
        }


    }
}