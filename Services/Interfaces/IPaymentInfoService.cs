using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Interfaces
{
    public interface IPaymentInfoService
    {
        ResponseHeader GetPaymentInfoByAccountId(int id);
        ResponseHeader InsertPaymentInfo(PaymentInfoDto paymentInfoDto);
        Task<ResponseHeader> AddPaymentTransaction(PaymentInfoDto model);
        ResponseHeader GetAllCardType();
    }
}
