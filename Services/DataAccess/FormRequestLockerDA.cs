using SmartLocker.Software.Backend.Models;
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
    public class FormRequestLockerDA : IFormRequestLockerDA
    {
        private readonly IBaseService baseService;
        private readonly IBaseRepository baseRepository;

        public FormRequestLockerDA(IBaseService baseService, IBaseRepository baseRepository)
        {
            this.baseService = baseService;
            this.baseRepository = baseRepository;
        }
        public PageModel<FormRequestDto> GetAllFormRequestByAccountId(int page, int perPage, int accountId, string keyWord = "")
        {
            string sql = $@"SELECT FR.FormId
                                  ,FR.FormCode
                                  ,FR.Status
                                  ,FR.CreateDate
                                  ,FR.UpdateDate
                                  ,FR.Remark
	                              ,LK.LockerCode
	                              ,LK.LockerId
	                              ,LC.LocateName
	                              ,LC.LocateId
	                              ,LC.Latitude
	                              ,LC.Longtitude
	                              ,A.SubDistrict
	                              ,A.District
	                              ,A.Province
	                              ,A.PostalCode
                              FROM [SmartLocker].[dbo].[FORM_REQUEST_LOCKERS] FR
                              INNER JOIN LOCKERS LK ON LK.LockerId = FR.LockerId 
                              INNER JOIN LOCATIONS LC ON LC.LocateId = LK.LocateId 
                              INNER JOIN ADDRESS A ON A.AddressId = LC.AddressId 
                              WHERE FR.AccountId = {accountId} 
                                    AND FR.Status <> 'I'
                                    AND (   FR.FormCode LIKE '%{keyWord}%' 
                                        OR LK.LockerCode LIKE '%{keyWord}%' 
                                        OR LC.LocateName LIKE '%{keyWord}%' 
                                        OR A.SubDistrict LIKE '%{keyWord}%'
                                        OR A.District LIKE '%{keyWord}%'
                                        OR A.PostalCode LIKE '%{keyWord}%' )
                              ORDER BY FR.CreateDate DESC ";
            return baseService.Pagination<FormRequestDto>(page, perPage, sql);
        }

        public PageModel<FormRequestDto> SearchFormRequest(int page, int perPage, FormRequestDto keyWord)
        {
            StringBuilder sqlString = new StringBuilder();
            sqlString.Append($@"SELECT FR.FormId
                                  , FR.FormCode
                                  , FR.Status
                                  , FR.CreateDate
                                  , FR.UpdateDate
                                  , FR.Remark
                                  , FR.OptionalRequest
                                  , LK.LockerCode
                                  , LK.LockerId
                                  , LC.LocateName
                                  , LC.LocateId
                                  , LC.Latitude
                                  , LC.Longtitude
                                  , A.SubDistrict
                                  , A.District
                                  , A.Province
                                  , A.PostalCode
                                  , ACC.FirstName
								  , ACC.LastName
                              FROM[SmartLocker].[dbo].[FORM_REQUEST_LOCKERS] FR
                              INNER JOIN ACCOUNTS ACC ON ACC.AccountId = FR.AccountId 
                              INNER JOIN LOCKERS LK ON LK.LockerId = FR.LockerId
                              INNER JOIN LOCATIONS LC ON LC.LocateId = LK.LocateId
                              INNER JOIN ADDRESS A ON A.AddressId = LC.AddressId  
                              WHERE 1=1 
                                AND FR.Status <> 'I' ");
            if (keyWord.AccountId > 0)
            {
                sqlString.Append(@$" AND FR.AccountId = {keyWord.AccountId} ");
            }
            sqlString.Append(@$" AND (  FR.FormCode LIKE '%{keyWord.FormCode}%' 
                                       AND  ACC.FirstName LIKE '%{keyWord.FirstName}%'
                                       AND  ACC.LastName LIKE '%{keyWord.LastName}%'
                                         AND LK.LockerCode LIKE '%{keyWord.LockerCode}%' 
                                         AND LC.LocateName LIKE '%{keyWord.LocateName}%' 
                                          AND A.SubDistrict LIKE '%{keyWord.SubDistrict}%'
                                          AND A.District LIKE '%{keyWord.District}%'
                                          AND A.PostalCode LIKE '%{keyWord.PostalCode}%' ");
            if (!String.IsNullOrEmpty(keyWord.Status))
            {
                sqlString.Append($@"  AND FR.Status = '{keyWord.Status}' ");
            }
            sqlString.Append($@" )
                              ORDER BY FR.CreateDate DESC ");
            return baseService.Pagination<FormRequestDto>(page, perPage, sqlString.ToString());
        }


        public int GetRoomAmountByLkSizeIdAndFormId(int lkSizeId, int formId)
        {
            string sql = $@"SELECT [Amount]
                                  FROM [SmartLocker].[dbo].[LOCKER_AMOUNT]
                                  WHERE [LkSizeId] = {lkSizeId} 
                                    AND [FormId] = {formId} 
                                    AND [Status] = 'A'";
            return baseRepository.QueryString<int>(sql).FirstOrDefault();
        }




    }
}
