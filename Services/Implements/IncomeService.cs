


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Constants;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Excel;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.Implements
{
    public class IncomeService : IIncomeService
    {
        private readonly IBaseRepository baseRepository;
        private readonly ILocationDA locationDA;
        private readonly IIncomeDA incomeDA;
        private readonly ILockerDA lockerDA;
        private readonly IBaseService baseService;

        public IncomeService(IBaseRepository baseRepository, ILocationDA locationDA, IBaseService baseService, IIncomeDA incomeDA, ILockerDA lockerDA)
        {
            this.baseRepository = baseRepository;
            this.locationDA = locationDA;
            this.baseService = baseService;
            this.incomeDA = incomeDA;
            this.lockerDA = lockerDA;
        }

        public ResponseHeader GetIncomeLocation(int page, int perPage, IncomeDto incomeDto)
        {
            IncomeDto incomeResult = new IncomeDto
            {
                Incomes = new List<Income>()
            };
            //check role for get location 
            string roleResult = baseService.CheckAccountRoleByAccountId(incomeDto.LocationDto.AccountId);

            //get location along role
            PageModel<LocationDto> locationResult;
            switch (roleResult)
            {
                case "400": throw new ArgumentException("AccountId is invalid.");
                case "409": throw new ConfilctDataException("Role is conflict data.");
                case "User": throw new ArgumentException("User cannot use this function");
                case "Admin":
                    incomeDto.LocationDto.AccountId = 0;
                    locationResult = locationDA.SearchLocation(page, perPage, incomeDto.LocationDto); break;
                case "Partner":
                    locationResult = locationDA.SearchLocation(page, perPage, incomeDto.LocationDto); break;
                default: throw new Exception("Something when wrong");
            };
            List<LocationDto> locationResultContent = locationResult.Content;


            //check start date and end date
            if (incomeDto.StartDate == null && incomeDto.EndDate == null)
            {
                throw new ArgumentException("this function cannot null neither StartDate nor EndDate ");
            }
            else if (incomeDto.StartDate == null && incomeDto.EndDate != null)
            {
                incomeDto.StartDate = AutoSetStartDateAndEndDate(durationType: incomeDto.DurationType, endDate: incomeDto.EndDate);
            }
            else if (incomeDto.StartDate != null && incomeDto.EndDate == null)
            {
                incomeDto.EndDate = AutoSetStartDateAndEndDate(durationType: incomeDto.DurationType, startDate: incomeDto.StartDate);
            }

            DateTime dateForIncome = incomeDto.EndDate.Value;
            //get Range title
            incomeResult.RangeTitle = incomeDto.DurationType switch
            {
                RangeGraph.Week => baseService.WeekLabel(dateForIncome),
                RangeGraph.Month => baseService.MonthLabel(dateForIncome),
                RangeGraph.Year => baseService.YearLabel(dateForIncome),
                _ => throw new ArgumentException("rangeGraphType is invalid"),
            };
            //get each Location's Income
            foreach (LocationDto location in locationResultContent)
            {
                dateForIncome = incomeDto.EndDate.Value;
                Income income = new Income
                {
                    LocateId = location.LocateId,
                    FirstName = location.FirstName,
                    LastName = location.LastName,
                    LocateName = location.LocateName,
                    IncomeResult = new double[7]
                };
                for (int i = 6; i >= 0; i--)
                {
                    income.IncomeResult[i] = incomeDA.GetIncome(date: dateForIncome, dateLength: incomeDto.DurationType, locateId: location.LocateId);
                    dateForIncome = incomeDto.DurationType switch
                    {
                        RangeGraph.Week => dateForIncome.AddDays(-1),
                        RangeGraph.Month => dateForIncome.AddMonths(-1),
                        RangeGraph.Year => dateForIncome.AddYears(-1),
                        _ => throw new ArgumentException("rangeGraphType is invalid"),
                    };
                }
                incomeResult.Incomes.Add(income);
            }
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location Income successful",
                Content = incomeResult,
                Page = locationResult.Page,
                PerPage = locationResult.PerPage,
                TotalElement = locationResult.TotalElement,
            };
        }

        public ResponseHeader GetIncomeLocker(int page, int perPage, IncomeLockerDto incomeLockerDto)
        {
            IncomeLockerDto incomeResult = new IncomeLockerDto
            {
                IncomeLockers = new List<IncomeLocker>(),
            };

            //check role for get locker 
            string roleResult = baseService.CheckAccountRoleByAccountId(incomeLockerDto.LockerFilter.AccountId.Value);

            //get locker along role
            PageModel<LockerDto> lockerResult;
            switch (roleResult)
            {
                case "400": throw new ArgumentException("AccountId is invalid.");
                case "409": throw new ConfilctDataException("Role is conflict data.");
                case "User": throw new ArgumentException("User cannot use this function");
                case "Admin":
                case "Partner":
                    lockerResult = lockerDA.GetLockerByFilter(page, perPage, incomeLockerDto.LockerFilter); break;

                default: throw new Exception("Something when wrong");
            };
            List<LockerDto> lockerResultContent = lockerResult.Content;

            //check start date and end date
            if (incomeLockerDto.StartDate == null && incomeLockerDto.EndDate != null)
            {
                incomeLockerDto.StartDate = AutoSetStartDateAndEndDate(durationType: incomeLockerDto.DurationType, endDate: incomeLockerDto.EndDate);
            }
            else if (incomeLockerDto.StartDate != null && incomeLockerDto.EndDate == null)
            {
                incomeLockerDto.EndDate = AutoSetStartDateAndEndDate(durationType: incomeLockerDto.DurationType, startDate: incomeLockerDto.StartDate);
            }

            DateTime dateForIncome = incomeLockerDto.EndDate.Value;
            //get Range title
            incomeResult.RangeTitle = incomeLockerDto.DurationType switch
            {
                RangeGraph.Week => baseService.WeekLabel(dateForIncome),
                RangeGraph.Month => baseService.MonthLabel(dateForIncome),
                RangeGraph.Year => baseService.YearLabel(dateForIncome),
                _ => throw new ArgumentException("rangeGraphType is invalid"),
            };
            //get each locker's Income
            foreach (LockerDto locker in lockerResultContent)
            {
                dateForIncome = incomeLockerDto.EndDate.Value;
                IncomeLocker income = new IncomeLocker
                {
                    LockerId = locker.LockerId,
                    LockerCode = locker.LockerCode,
                    IncomeResult = new double[7]
                };
                for (int i = 6; i >= 0; i--)
                {
                    income.IncomeResult[i] = incomeDA.GetIncome(date: dateForIncome, dateLength: incomeLockerDto.DurationType, lockerId: locker.LockerId);
                    dateForIncome = incomeLockerDto.DurationType switch
                    {
                        RangeGraph.Week => dateForIncome.AddDays(-1),
                        RangeGraph.Month => dateForIncome.AddMonths(-1),
                        RangeGraph.Year => dateForIncome.AddYears(-1),
                        _ => throw new ArgumentException("rangeGraphType is invalid"),
                    };
                }
                incomeResult.IncomeLockers.Add(income);
            }
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Locker Income successful",
                Content = incomeResult,
                Page = lockerResult.Page,
                PerPage = lockerResult.PerPage,
                TotalElement = lockerResult.TotalElement,
            };
        }

        public ResponseHeader GetIncomeLockerRoom(IncomeLockerRoomDto incomeLockerRoomDto)
        {
            IncomeLockerRoomDto incomeResult = new IncomeLockerRoomDto
            {
                IncomeLockerRooms = new List<IncomeLockerRoom>(),
            };

            //check role for get locker 
            string roleResult = baseService.CheckAccountRoleByAccountId(incomeLockerRoomDto.AccountId.Value);

            //get locker room along role
            List<LockerRoomDto> lockerRoomResultContent;
            switch (roleResult)
            {
                case "400": throw new ArgumentException("AccountId is invalid.");
                case "409": throw new ConfilctDataException("Role is conflict data.");
                case "User": throw new ArgumentException("User cannot use this function");
                case "Admin":
                case "Partner":
                    lockerRoomResultContent = lockerDA.GetAllLockerRoom(new LockerRoomDto() { LockerId = incomeLockerRoomDto.LockerId }); break;

                default: throw new Exception("Something when wrong");
            };


            //check start date and end date
            if (incomeLockerRoomDto.StartDate == null && incomeLockerRoomDto.EndDate != null)
            {
                incomeLockerRoomDto.StartDate = AutoSetStartDateAndEndDate(durationType: incomeLockerRoomDto.DurationType, endDate: incomeLockerRoomDto.EndDate);
            }
            else if (incomeLockerRoomDto.StartDate != null && incomeLockerRoomDto.EndDate == null)
            {
                incomeLockerRoomDto.EndDate = AutoSetStartDateAndEndDate(durationType: incomeLockerRoomDto.DurationType, startDate: incomeLockerRoomDto.StartDate);
            }

            DateTime dateForIncome = incomeLockerRoomDto.EndDate.Value;
            //get Range title
            incomeResult.RangeTitle = incomeLockerRoomDto.DurationType switch
            {
                RangeGraph.Week => baseService.WeekLabel(dateForIncome),
                RangeGraph.Month => baseService.MonthLabel(dateForIncome),
                RangeGraph.Year => baseService.YearLabel(dateForIncome),
                _ => throw new ArgumentException("rangeGraphType is invalid"),
            };
            //get each locker's Income
            foreach (LockerRoomDto lockerRoom in lockerRoomResultContent)
            {
                dateForIncome = incomeLockerRoomDto.EndDate.Value;
                IncomeLockerRoom income = new IncomeLockerRoom
                {
                    LockerRoomCode = lockerRoom.LkRoomCode,
                    IncomeResult = new double[7]
                };
                for (int i = 6; i >= 0; i--)
                {
                    income.IncomeResult[i] = incomeDA.GetIncome(date: dateForIncome, dateLength: incomeLockerRoomDto.DurationType, lkRoomId: lockerRoom.LkRoomId);
                    dateForIncome = incomeLockerRoomDto.DurationType switch
                    {
                        RangeGraph.Week => dateForIncome.AddDays(-1),
                        RangeGraph.Month => dateForIncome.AddMonths(-1),
                        RangeGraph.Year => dateForIncome.AddYears(-1),
                        _ => throw new ArgumentException("rangeGraphType is invalid"),
                    };
                }
                incomeResult.IncomeLockerRooms.Add(income);
            }
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Locker Income successful",
                Content = incomeResult,
                Page = 1,
                PerPage = Int32.MaxValue,
                TotalElement = lockerRoomResultContent.Count,
            };
        }
        private DateTime AutoSetStartDateAndEndDate(string durationType, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null && endDate == null)
            {
                throw new ArgumentException("this function cannot null neither start date nor end date");
            }
            //for startDate AutoSet
            else if (startDate == null)
            {
                return durationType switch
                {
                    RangeGraph.Week => endDate.Value.AddDays(-6),
                    RangeGraph.Month => endDate.Value.AddMonths(-6),
                    RangeGraph.Year => endDate.Value.AddYears(-6),
                    _ => throw new ArgumentException("rangeGraphType is invalid"),
                };
            }
            //for endDate AutoSet
            else if (endDate == null)
            {
                return durationType switch
                {
                    RangeGraph.Week => startDate.Value.AddDays(6),
                    RangeGraph.Month => startDate.Value.AddMonths(6),
                    RangeGraph.Year => startDate.Value.AddYears(6),
                    _ => throw new ArgumentException("rangeGraphType is invalid"),
                };
            }
            else
            {
                throw new ArgumentException("this function cannot get date neither start date nor end date");
            }
        }

        public string GetIncomeLocationExcel(int page, int perPage, IncomeDto income)
        {
            ResponseHeader result = GetIncomeLocation(page, perPage, income);
            var content = result.Content as IncomeDto;

            //set data to create spreadsheet
            string fileName = "รายงานรายได้พื้นที่" + " " + DateTime.Now.ToString("dd_MMMM_yyyy", new CultureInfo("th-TH")) + ".xlsx";
            string path = "wwwroot/reports/" + fileName;
            string worksheetName = "รายได้พื้นที่";

            //create spreadsheet
            SpreadSheetModel spreadSheet = new SpreadSheetModel(path, worksheetName);

            //intial row index
            uint row = 0;
            
            //set filter result
            spreadSheet.CreateCell("คำค้นหา",row++,0);
            spreadSheet.CreateCell("ชื่อสถานที่", row,0);
            spreadSheet.CreateCell(income.LocationDto.LocateName ?? "-", row++,1);
            if (income.LocationDto.AccountId == 0)
            {
                spreadSheet.CreateCell("ชื่อ", row, 0);
                spreadSheet.CreateCell(income.LocationDto.FirstName ?? "-", row++, 1);
                spreadSheet.CreateCell("สกุล", row, 0);
                spreadSheet.CreateCell(income.LocationDto.LastName ?? "-", row++, 1);
            }
            spreadSheet.CreateCell("ตำบล", row, 0);
            spreadSheet.CreateCell(income.LocationDto.SubDistrict ?? "-", row++, 1);
            spreadSheet.CreateCell("อำเภอ", row, 0);
            spreadSheet.CreateCell(income.LocationDto.District ?? "-", row++, 1);
            spreadSheet.CreateCell("จังหวัด", row, 0);
            spreadSheet.CreateCell(income.LocationDto.Province ?? "-", row++, 1);
            spreadSheet.CreateCell("เลขไปรษณีย์", row, 0);
            spreadSheet.CreateCell(income.LocationDto.PostalCode ?? "-", row++, 1);
            spreadSheet.CreateCell("ช่วงเวลา", row, 0);
            spreadSheet.CreateCell(income.DurationType switch {
                RangeGraph.Week => "รายสัปดาห์",
                RangeGraph.Month => "รายเดือน",
                RangeGraph.Year => "รายปี",
                _ => "-"
            }, row++, 1);
            string startdate;
            string enddate;
            switch (income.DurationType)
            {
                case RangeGraph.Week:
                    startdate = income.StartDate.Value.ToString("dd MMMM yyyy", new CultureInfo("th-TH"));
                    enddate = income.EndDate.Value.ToString("dd MMMM yyyy", new CultureInfo("th-TH")); break;
                case RangeGraph.Month:
                    startdate = income.StartDate.Value.ToString("MMMM yyyy", new CultureInfo("th-TH"));
                    enddate = income.EndDate.Value.ToString("MMMM yyyy", new CultureInfo("th-TH")); break;
                case RangeGraph.Year:
                    startdate = income.StartDate.Value.ToString("yyyy", new CultureInfo("th-TH"));
                    enddate = income.EndDate.Value.ToString("yyyy", new CultureInfo("th-TH")); break;
                default: throw new Exception("cannot convert startdate and enddate");
            }
            spreadSheet.CreateCell("วันเริ่มต้น", row, 0);
            spreadSheet.CreateCell(startdate, row++, 1);
            spreadSheet.CreateCell("วันสิ้นสุด", row, 0);
            spreadSheet.CreateCell(enddate, row++, 1);
            row += 1;

            //set header data
            List<string> headers = new List<string> { "ชื่อสถานที่", "ชื่อ", "นามสกุล" };
            foreach (string title in content.RangeTitle)
            {
                headers.Add(title);
            }
            headers.Add("รวม");
            for (int i = 0; i < headers.Count; i++)
            {
                spreadSheet.CreateCell(headers[i], row, i);
            }
            row += 1;

            //set data 
            for (int i = 0; i < content.Incomes.Count; i++)
            {
                double sumIncomeThisLocation = 0;
                var locationIncome = content.Incomes[i];
                spreadSheet.CreateCell(locationIncome.LocateName, row, 0);
                spreadSheet.CreateCell(locationIncome.FirstName, row, 1);
                spreadSheet.CreateCell(locationIncome.LastName, row, 2);
                for (int j = 0; j < locationIncome.IncomeResult.Length; j++)
                {
                    spreadSheet.CreateCell(locationIncome.IncomeResult[j].ToString(), row, j + 3 , CellValues.Number);
                    sumIncomeThisLocation += locationIncome.IncomeResult[j];
                }
                spreadSheet.CreateCell(sumIncomeThisLocation.ToString(), row, 10 , CellValues.Number);

                row++;
            }

            //save and close file
            spreadSheet.Save();
            spreadSheet.Close();
            return fileName;
        }
        public string GetIncomeLockerExcel(int page, int perPage, IncomeLockerDto incomeLockerDto)
        {
            ResponseHeader result = GetIncomeLocker(page, perPage, incomeLockerDto);
            var content = result.Content as IncomeLockerDto;
            using var context = new SmartLockerContext();
            string locateName = context.Locations.Where(c => c.LocateId == incomeLockerDto.LockerFilter.LocateId).FirstOrDefault().LocateName;

            //set data to create spreadsheet
            string fileName = "รายงานรายได้ล็อคเกอร์-" + DateTime.Now.ToString("dd_MMMM_yyyy", new CultureInfo("th-TH")) + ".xlsx";
            string path = "wwwroot/reports/" + fileName;
            string worksheetName = "รายได้ล็อคเกอร์";

            //create spreadsheet
            SpreadSheetModel spreadSheet = new SpreadSheetModel(path, worksheetName);

            //initial row index
            uint row = 0;

            //set filter result
            spreadSheet.CreateCell("คำค้นหา", row++, 0);
            spreadSheet.CreateCell("ชื่อสถานที่", row, 0);
            spreadSheet.CreateCell(locateName ?? "-", row++, 1);
            spreadSheet.CreateCell("รหัสล็อคเกอร์", row, 0);
            spreadSheet.CreateCell(incomeLockerDto.LockerFilter.LockerCode ?? "-", row++, 1);
            spreadSheet.CreateCell("ช่วงเวลา", row, 0);
            spreadSheet.CreateCell(incomeLockerDto.DurationType switch
            {
                RangeGraph.Week => "รายสัปดาห์",
                RangeGraph.Month => "รายเดือน",
                RangeGraph.Year => "รายปี",
                _ => "-"
            }, row++, 1);

            string startdate;
            string enddate;
            switch (incomeLockerDto.DurationType)
            {
                case RangeGraph.Week:
                    startdate = incomeLockerDto.StartDate.Value.ToString("dd MMMM yyyy", new CultureInfo("th-TH"));
                    enddate = incomeLockerDto.EndDate.Value.ToString("dd MMMM yyyy", new CultureInfo("th-TH")); break;
                case RangeGraph.Month:
                    startdate = incomeLockerDto.StartDate.Value.ToString("MMMM yyyy", new CultureInfo("th-TH"));
                    enddate = incomeLockerDto.EndDate.Value.ToString("MMMM yyyy", new CultureInfo("th-TH")); break;
                case RangeGraph.Year:
                    startdate = incomeLockerDto.StartDate.Value.ToString("yyyy", new CultureInfo("th-TH"));
                    enddate = incomeLockerDto.EndDate.Value.ToString("yyyy", new CultureInfo("th-TH")); break;
                default: throw new Exception("cannot convert startdate and enddate");
            }
            spreadSheet.CreateCell("วันเริ่มต้น", row, 0);
            spreadSheet.CreateCell(startdate, row++, 1);
            spreadSheet.CreateCell("วันสิ้นสุด", row, 0);
            spreadSheet.CreateCell(enddate, row++, 1);

            row += 1;

            //set header data
            List<string> headers = new List<string> { "รหัสล็อคเกอร์" };
            foreach (string title in content.RangeTitle)
            {
                headers.Add(title);
            }
            headers.Add("รวม");
            for (int i = 0; i < headers.Count; i++)
            {
                spreadSheet.CreateCell(headers[i], row, i);
            }
            row += 1;

            //set data
            for (int i = 0; i < content.IncomeLockers.Count; i++)
            {
                double sumIncomeThisLocker = 0;
                var lockerIncome = content.IncomeLockers[i];
                spreadSheet.CreateCell(lockerIncome.LockerCode, row, 0);
                for (int j = 0; j < lockerIncome.IncomeResult.Length; j++)
                {
                    spreadSheet.CreateCell(lockerIncome.IncomeResult[j].ToString(), row, j + 1, CellValues.Number);
                    sumIncomeThisLocker += lockerIncome.IncomeResult[j];
                }
                spreadSheet.CreateCell(sumIncomeThisLocker.ToString(), row, (lockerIncome.IncomeResult.Length + 1), CellValues.Number);
                row++;
            }

            //save and close
            spreadSheet.Save();
            spreadSheet.Close();
            return fileName;
        }
        public string GetIncomeLockerRoomExcel(IncomeLockerRoomDto incomeLockerRoomDto)
        {
            ResponseHeader result = GetIncomeLockerRoom(incomeLockerRoomDto);
            var content = result.Content as IncomeLockerRoomDto;
            using var context = new SmartLockerContext();
            string lockerCode = context.Lockers.Where(c => c.LockerId == incomeLockerRoomDto.LockerId).FirstOrDefault().LockerCode;

            //set data to create spreadsheet
            string fileName = "รายงานรายได้ช่องล็อคเกอร์-" + DateTime.Now.ToString("dd_MMMM_yyyy", new CultureInfo("th-TH")) + ".xlsx";
            string path = "wwwroot/reports/" + fileName;
            string worksheetName = "รายได้ช่องล็อคเกอร์";

            //create spreadsheet
            SpreadSheetModel spreadSheet = new SpreadSheetModel(path, worksheetName);

            //initial row index
            uint row = 0;

            //set filter result
            spreadSheet.CreateCell("คำค้นหา", row++, 0);
            spreadSheet.CreateCell("รหัสล็อคเกอร์", row, 0);
            spreadSheet.CreateCell(lockerCode ?? "-", row++, 1);
            spreadSheet.CreateCell("ช่วงเวลา", row, 0);
            spreadSheet.CreateCell(incomeLockerRoomDto.DurationType switch
            {
                RangeGraph.Week => "รายสัปดาห์",
                RangeGraph.Month => "รายเดือน",
                RangeGraph.Year => "รายปี",
                _ => "-"
            }, row++, 1);
            string startdate;
            string enddate;
            switch (incomeLockerRoomDto.DurationType)
            {
                case RangeGraph.Week:
                    startdate = incomeLockerRoomDto.StartDate.Value.ToString("dd MMMM yyyy", new CultureInfo("th-TH"));
                    enddate = incomeLockerRoomDto.EndDate.Value.ToString("dd MMMM yyyy", new CultureInfo("th-TH")); break;
                case RangeGraph.Month:
                    startdate = incomeLockerRoomDto.StartDate.Value.ToString("MMMM yyyy", new CultureInfo("th-TH"));
                    enddate = incomeLockerRoomDto.EndDate.Value.ToString("MMMM yyyy", new CultureInfo("th-TH")); break;
                case RangeGraph.Year:
                    startdate = incomeLockerRoomDto.StartDate.Value.ToString("yyyy", new CultureInfo("th-TH"));
                    enddate = incomeLockerRoomDto.EndDate.Value.ToString("yyyy", new CultureInfo("th-TH")); break;
                default: throw new Exception("cannot convert startdate and enddate");
            }
            spreadSheet.CreateCell("วันเริ่มต้น", row, 0);
            spreadSheet.CreateCell(startdate, row++, 1);
            spreadSheet.CreateCell("วันสิ้นสุด", row, 0);
            spreadSheet.CreateCell(enddate, row++, 1);
            row += 1;

            //set header data
            List<string> headers = new List<string> { "รหัสช่องล็อคเกอร์" };
            foreach (string title in content.RangeTitle)
            {
                headers.Add(title);
            }
            headers.Add("รวม");
            for (int i = 0; i < headers.Count; i++)
            {
                spreadSheet.CreateCell(headers[i], row, i);
            }
            row += 1;

            //set data
            for (int i = 0; i < content.IncomeLockerRooms.Count; i++)
            {
                double sumIncomeThisLocker = 0;
                var lockerIncome = content.IncomeLockerRooms[i];
                spreadSheet.CreateCell(lockerIncome.LockerRoomCode, row, 0);
                for (int j = 0; j < lockerIncome.IncomeResult.Length; j++)
                {
                    spreadSheet.CreateCell(lockerIncome.IncomeResult[j].ToString(), row, j + 1, CellValues.Number);
                    sumIncomeThisLocker += lockerIncome.IncomeResult[j];
                }
                spreadSheet.CreateCell(sumIncomeThisLocker.ToString(), row, (lockerIncome.IncomeResult.Length + 1), CellValues.Number);
                row++;
            }

            //save and close file
            spreadSheet.Save();
            spreadSheet.Close();
            return fileName;
        }
    }
}
