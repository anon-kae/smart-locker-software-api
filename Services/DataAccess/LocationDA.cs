using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;
using SmartLocker.Software.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess
{
    public class LocationDA : ILocationDA
    {
        private readonly IBaseService baseService;
        public LocationDA(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public PageModel<LocationDto> GetLocationAll(GetAmountDataDto getAmount)
        {
            string query = $@"SELECT L.*,A.Province,A.District,A.PostalCode,A.SubDistrict,AC.FirstName,AC.LastName
                                FROM [SmartLocker].[dbo].[LOCATIONS] L
                                INNER JOIN ADDRESS A ON L.AddressId = A.AddressId
                                INNER JOIN ACCOUNTS AC ON L.AccountId = AC.AccountId
                                WHERE L.[Status] = 'W'
                                ORDER BY CreateDate DESC";
            PageModel<LocationDto> pageModel = baseService.Pagination<LocationDto>(getAmount.Page, getAmount.PerPage, query);
            return pageModel;
        }
        public PageModel<LocationDto> SearchLocation(int page , int perPage , LocationDto keyword)
        {
            StringBuilder sqlString = new StringBuilder();
            sqlString.Append(@$"SELECT LO.[LocateId]
                                      ,LO.[LocateName]
                                      ,LO.[Longtitude]
                                      ,LO.[Latitude]
                                      ,LO.[AddressId]
                                      ,LO.[Status]
                                      ,LO.[CreateDate]
                                      ,LO.[UpdateDate]
                                      ,LO.[AccountId]
                                      ,LO.[ApproveDate]
                                      ,LO.[Remark]
	                                  ,ACC.FirstName
	                                  ,ACC.LastName
	                                  ,AD.SubDistrict
	                                  ,AD.District
	                                  ,AD.Province
	                                  ,AD.PostalCode
                                  FROM [SmartLocker].[dbo].[LOCATIONS] LO
                                  INNER JOIN ADDRESS AD ON LO.AddressId = AD.AddressId
                                  INNER JOIN ACCOUNTS ACC ON LO.AccountId = ACC.AccountId
                                  WHERE 1=1
                                  AND LO.Status <> 'I' ");

            if(keyword.AccountId > 0)
            {
                sqlString.Append($@"  AND ACC.AccountId = {keyword.AccountId} ");
            }
            sqlString.Append(@$" AND (
	                                LO.LocateName LIKE '%{keyword.LocateName}%'
	                                AND ACC.FirstName LIKE '%{keyword.FirstName}%'
	                                AND ACC.LastName LIKE '%{keyword.LastName}%'
	                                AND AD.SubDistrict LIKE '%{keyword.SubDistrict}%'
	                                AND AD.District LIKE '%{keyword.District}%'
	                                AND AD.Province LIKE '%{keyword.Province}%' 
	                                AND AD.PostalCode LIKE '%{keyword.PostalCode}%' 
	                                ");
            if (!String.IsNullOrEmpty(keyword.Status))
            {
                sqlString.Append($@"AND LO.Status = '{keyword.Status}' ");
            }
            sqlString.Append(@$" )
                                  ORDER BY LO.[LocateId] DESC");

            PageModel<LocationDto> pageModel = baseService.Pagination<LocationDto>(page, perPage, sqlString.ToString());
            return pageModel;
        }
    }
}
