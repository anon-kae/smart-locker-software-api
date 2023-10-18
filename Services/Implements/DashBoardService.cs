
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.Dashboard;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IDashboardDA dashboardDA;
        private readonly IBaseService baseService;
        public DashBoardService(IDashboardDA dashboardDA , IBaseService baseService)
        {
            this.dashboardDA = dashboardDA;
            this.baseService = baseService;
        }

        public ResponseHeader GetLocateAndLockerResultByAccountId(int AccountId)
        {
            //Check Account first
            string checkAccountResult = baseService.CheckAccountRoleByAccountId(AccountId);
            ResponseHeader responseHeader = new ResponseHeader
            {
                Content = checkAccountResult switch
                {
                    "400" => throw new ArgumentException("AccountId is invalid."),
                    "409" => throw new ConfilctDataException("Role is conflict data."),
                    "User" => throw new ArgumentException("User cannot use this function"),
                    "Admin" => GetAllLocateAndLockerData(),
                    "Partner" => GetLocateAndLockerDataByAccountId(AccountId),
                    _ => throw new Exception("Something when wrong"),
                },
                Status = "S",
                Message = $@"Get locate and locker {checkAccountResult} successful."
            };
            return responseHeader;
        }
        private LocationAndLockerResult GetAllLocateAndLockerData()
        {
            LocationAndLockerResult locationAndLocker = new LocationAndLockerResult
            {
                LockerAmount = dashboardDA.LockerAllCount(),
                LockerNRAmount = dashboardDA.LockerNRCount(),
                LockerRepairAmount = dashboardDA.LockerRepairCount(),
                LocationAmount = dashboardDA.LocationAllCount(),
                LocationNRAmount = dashboardDA.LocationNRCount()
            };
            return locationAndLocker;
        }
        private LocationAndLockerResult GetLocateAndLockerDataByAccountId(int AccountId)
        {
            LocationAndLockerResult locationAndLocker = new LocationAndLockerResult
            {
                LockerAmount = dashboardDA.LockerAllCount(AccountId),
                LockerNRAmount = dashboardDA.LockerNRCount(AccountId),
                LockerRepairAmount = dashboardDA.LockerRepairCount(AccountId),
                LocationAmount = dashboardDA.LocationAllCount(AccountId),
                LocationNRAmount = dashboardDA.LocationNRCount(AccountId)
            };
            return locationAndLocker;
        }

        public ResponseHeader IncomeResult(DashboardRequireModel dashboardRequireModel)
        {
            
            //Check Account first
            string checkAccountResult = baseService.CheckAccountRoleByAccountId(dashboardRequireModel.AccountId);
            
            ResponseHeader responseHeader = new ResponseHeader
            {
                Content = checkAccountResult switch
                {
                    "400" => throw new ArgumentException("AccountId is invalid."),
                    "409" => throw new ConfilctDataException("Role is conflict data."),
                    "User" => throw new ArgumentException("User cannot use this function"),
                    "Admin" => AllIncome(dashboardRequireModel),
                    "Partner" => PartnerIncome(dashboardRequireModel),
                    _ => throw new Exception("Something when wrong"),
                },
                Status = "S",
                Message = @$"Get Income {checkAccountResult} successful."
            };
            return responseHeader;
        }
        private IncomeDashboard PartnerIncome(DashboardRequireModel dashboardRequireModel)
        {
            using var context = new SmartLockerContext();
            var rateTypeResult = context.RateTypes.ToArray();

            IncomeDashboard incomeDashboard = new IncomeDashboard
            {
                Label = GetRangeName(dashboardRequireModel.RangeGraphType),
                RateTypeName = new string[rateTypeResult.Length],
                Value = new Decimal[rateTypeResult.Length][]
            };

            for (int i = 0; i < rateTypeResult.Length; i++)
            {
                incomeDashboard.RateTypeName[i] = rateTypeResult[i].TypeName;
                incomeDashboard.Value[i] = dashboardDA.IncomeData(dashboardRequireModel.RangeGraphType, rateTypeResult[i].TypeId, dashboardRequireModel.AccountId);
            }
            return incomeDashboard;
        }
        private IncomeDashboard AllIncome(DashboardRequireModel dashboardRequireModel)
        {
            using var context = new SmartLockerContext();
            var rateTypeResult = context.RateTypes.ToArray();

            IncomeDashboard incomeDashboard = new IncomeDashboard
            {
                Label = GetRangeName(dashboardRequireModel.RangeGraphType),
                RateTypeName = new string[rateTypeResult.Length],
                Value = new Decimal[rateTypeResult.Length][]
            };

            for (int i = 0; i < rateTypeResult.Length; i++)
            {
                incomeDashboard.RateTypeName[i] = rateTypeResult[i].TypeName;
                incomeDashboard.Value[i] = dashboardDA.IncomeData(dashboardRequireModel.RangeGraphType, rateTypeResult[i].TypeId);
            }
            return incomeDashboard;
        }
        private string[] GetRangeName(string rangeGraphType)
        {
            return rangeGraphType switch
            {
                RangeGraph.Week => baseService.WeekLabel(),
                RangeGraph.Month => baseService.MonthLabel(),
                RangeGraph.Year => baseService.YearLabel(),
                _ => throw new ArgumentException("rangeGraphType is invalid"),
            };
        }


        public ResponseHeader GetBookingLockerCountByAccountId(DashboardRequireModel dashboardRequireModel)
        {
            //Check Account first
            string checkAccountResult = baseService.CheckAccountRoleByAccountId(dashboardRequireModel.AccountId);
            ResponseHeader responseHeader = new ResponseHeader
            {
                Content = checkAccountResult switch
                {
                    "400" => throw new ArgumentException("AccountId is invalid."),
                    "409" => throw new ConfilctDataException("Role is conflict data."),
                    "User" => throw new ArgumentException("User cannot use this function"),
                    "Admin" => dashboardDA.GetBookingLockerCountByAccountId(dashboardRequireModel.RangeGraphType,dashboardRequireModel.MonthRange),
                    "Partner" => dashboardDA.GetBookingLockerCountByAccountId(dashboardRequireModel.RangeGraphType, dashboardRequireModel.MonthRange , dashboardRequireModel.AccountId),
                    _ => throw new Exception("Something when wrong"),
                },
                Status = "S",
                Message = @$"Get Booking count locker {checkAccountResult} successful."
            };
            return responseHeader;
        }
        public ResponseHeader GetBookingLocationCountByAccountId(DashboardRequireModel dashboardRequireModel)
        {
            //Check Account first
            string checkAccountResult = baseService.CheckAccountRoleByAccountId(dashboardRequireModel.AccountId);
            ResponseHeader responseHeader = new ResponseHeader
            {
                Content = checkAccountResult switch
                {
                    "400" => throw new ArgumentException("AccountId is invalid."),
                    "409" => throw new ConfilctDataException("Role is conflict data."),
                    "User" => throw new ArgumentException("User cannot use this function"),
                    "Admin" => dashboardDA.GetBookingLocationCountByAccountId(dashboardRequireModel.RangeGraphType, dashboardRequireModel.MonthRange),
                    "Partner" => dashboardDA.GetBookingLocationCountByAccountId(dashboardRequireModel.RangeGraphType, dashboardRequireModel.MonthRange, dashboardRequireModel.AccountId),
                    _ => throw new Exception("Something when wrong"),
                },
                Status = "S",
                Message = @$"Get Booking count location {checkAccountResult} successful."
            };
            return responseHeader;
        }


    }
}
