using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess
{
    public class PaymentInfoDA : IPaymentInfoDA
    {
        public int AddAccountPaymentInfo(AccountPaymentInfo accountPaymentInfo)
        {
            using var context = new SmartLockerContext();
            
            context.AccountPaymentInfo.Add(accountPaymentInfo);
            int validateInsert = context.SaveChanges();
            return validateInsert;
        }

        public List<AccountPaymentInfo> getAccountPaymentInfoByAccountId(int id)
        {
            using var context = new SmartLockerContext();
            var accPaymentInfoResult = context.AccountPaymentInfo.Where(c => c.AccountId == id && c.Status == "A").ToList();
            return accPaymentInfoResult;
        }
    }
}
