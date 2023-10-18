using AutoMapper;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omise;
using Omise.Models;
using smartlocker.software.api.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Repositories.Interfaces;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class PaymentInfoService : IPaymentInfoService
    {
        private readonly IPaymentInfoDA paymentInfoDA;
        private readonly IMapper mapper;
        private readonly ISqlDA sql;
        private readonly IBaseRepository baseRepository;
        private readonly Client client;
        private readonly string PublicKey = "pkey_test_5lk9eu74kzb3ggevga2";
        private readonly string Secretkey = "skey_test_5lk9eu74uyzpfy8rd4q";

        public string GenerateOrderID()
        {
            Random rnd = new Random();
            Int64 s1 = rnd.Next(000000, 999999);
            Int64 s2 = Convert.ToInt64(DateTime.Now.ToString("ddMMyyyyHHmmss"));
            string s3 = s1.ToString() + "" + s2.ToString();
            return s3;
        }
        public PaymentInfoService(IPaymentInfoDA paymentInfoDA, IMapper mapper, ISqlDA sql, IBaseRepository baseRepository)
        {
            this.paymentInfoDA = paymentInfoDA;
            this.mapper = mapper;
            this.client = new Client(this.PublicKey, this.Secretkey);
            this.sql = sql;
            this.baseRepository = baseRepository;
        }
        public ResponseHeader GetPaymentInfoByAccountId(int id)
        {
            var paymentInfoResult = paymentInfoDA.getAccountPaymentInfoByAccountId(id);
            List<PaymentInfoDto> infoDtos = mapper.Map<List<AccountPaymentInfo>, List<PaymentInfoDto>>(paymentInfoResult);

            foreach(PaymentInfoDto info in infoDtos)
            {
                var number = new StringBuilder(info.CardNumber);
                // number.Remove(6, 6);
                // number.Insert(6, "xxxxxx");
                info.CardNumber = number.ToString();
            }
            return new ResponseHeader
            {
                Status = "S",
                Message = "Get AccountPaymentInfo By AccountId",
                Content = infoDtos
            };
        }

#warning Payment Gateway is close so don't forget to open to full system
        public async Task<ResponseHeader> AddPaymentTransaction(PaymentInfoDto model)
        {
            //var spiltDate = model.ExpDate.ToShortDateString().Split('/');
            //int ExpirationMonth = Convert.ToInt32(spiltDate[0]);
            //int ExpirationYear = Convert.ToInt32(spiltDate[2]);
            //var amount = model.Amount.ToString();
            //var newAmount = amount + "00";
            //var token = await client.Tokens.Create(new CreateTokenRequest
            //{
            //    Name = model.CardName,
            //    Number = model.CardNumber,
            //    SecurityCode = model.CVV,
            //    ExpirationMonth = ExpirationMonth,
            //    ExpirationYear = ExpirationYear
            //});
            //var charge = await client.Charges.Create(new CreateChargeRequest
            //{
            //    Amount = Convert.ToInt32(newAmount),
            //    Currency = "THB",
            //    Card = token.Id
            //});
            string transactionNumber = GenerateOrderID();
            var Amount = Convert.ToDecimal(model.Amount);
            string queryString = sql.InsertTransaction(transactionNumber,  Amount , model.RateTypeId, model.AccountId , model.LkRoomId);
            var result = baseRepository.ExecuteString<int>(queryString);
            if (result != 0)
            {
                return new ResponseHeader()
                {
                    Status = "S",
                    Message = "Payment success",
                    Content = transactionNumber
                };
            }
            else
            {
                return new ResponseHeader()
                {
                    Status = "F",
                    Message = "Payment unsuccess",
                    Content = null
                };
            }

            // var paymentInfoResult = paymentInfoDA.getAccountPaymentInfoByAccountId(id);
            // List<PaymentInfoDto> infoDtos = mapper.Map<List<AccountPaymentInfo>, List<PaymentInfoDto>>(paymentInfoResult);
            // return new ResponseHeader
            // {
            //     Status = "S",
            //     Message = "Get AccountPaymentInfo By AccountId",
            //     Content = infoDtos
            // };
        }

        public ResponseHeader InsertPaymentInfo(PaymentInfoDto paymentInfoDto)
        {
            
            AccountPaymentInfo paymentInfo = mapper.Map<PaymentInfoDto, AccountPaymentInfo>(paymentInfoDto);
            paymentInfo.CreateDate = DateTime.Now;
            paymentInfo.Status = "A";
            var result = paymentInfoDA.AddAccountPaymentInfo(paymentInfo);
            if (result == 0)
            {
                throw new Exception("Can't insert");
            }
            else
            {
                return new ResponseHeader
                {
                    Status = "S",
                    Message = "insert sucessful",
                    Content = null
                };
            }
        }
        public ResponseHeader GetAllCardType()
        {
            using var context = new SmartLockerContext();
            List<CardType> cardTypeList = context.CardType.Where(ct => ct.Status == "A").ToList();
            if (cardTypeList != null)
            {

                return new ResponseHeader
                {
                    Status = "S",
                    Message = "Get All CardType sucessful",
                    Content = cardTypeList
                };
            }
            else
            {
                throw new Exception("Can't getData");

            }
        }
    }
}
