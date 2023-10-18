using System;

namespace smartlocker.software.api.Models
{
    public class LocationDto
    {
        public int LocateId { get; set; }
        public string LocateName { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }
        public int AccountId { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }



        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string District { get; set; }
        public string SubDistrict { get; set; }
    }
}