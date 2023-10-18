using smartlocker.software.api.Models;

namespace smartlocker.software.api.Services.DataAccess.Interface
{
    public interface ISqlDA
    {
        string selectLocation();
        string selectLocationByAccountId(int page, int perPage,int id);
        string selectLocationById(int id);
        string searchLocation(string keywords);
        string InsertNotification(NotificationDto notification);
        string InsertTransaction(string transactionNumber, decimal Amount, int? RateTimeId, int AccountId , int? LkRoomId);
    }
}