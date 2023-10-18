using System;
using System.Collections.Generic;
using System.Linq;
using smartlocker.software.api.Models;
using smartlocker.software.api.Services.DataAccess.Interface;
using smartlocker.software.api.Services.Interfaces;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using SmartLocker.Software.Backend.Models.Output.ErrorResponse;
using SmartLocker.Software.Backend.Repositories.Interfaces;
using SmartLocker.Software.Backend.Services.DataAccess.Interface;

namespace smartlocker.software.api.Services.Implements
{
    public class LocationService : ILocationService
    {
        private readonly IBaseRepository baseRepository;
        private readonly ISqlDA sql;
        private readonly ILocationDA locationDA;

        public LocationService(IBaseRepository baseRepository, ISqlDA sql, ILocationDA locationDA)
        {
            this.baseRepository = baseRepository;
            this.sql = sql;
            this.locationDA = locationDA;
        }

        public ResponseHeader GetLocation()
        {
            string queryString = sql.selectLocation();
            var result = baseRepository
                        .QueryString<LocationDto>(queryString)
                        .ToList();

            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location sucessful",
                Content = result,
                Page = 1,
                PerPage = int.MaxValue,
                TotalElement = result.Count
            };
        }

        public ResponseHeader GetLocationById(int id)
        {
            string queryString = sql.selectLocationById(id);
            var result = baseRepository
                        .QueryString<LocationDto>(queryString)
                        .FirstOrDefault();

            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location by Id sucessful",
                Content = result,
                Page = 1,
                PerPage = 1,
                TotalElement = 1
            };

        }
        public ResponseHeader GetLocationByAccountId(int page, int perPage, int id)
        {
            string queryString = sql.selectLocationByAccountId(page, perPage, id);
            var result = baseRepository
                        .QueryString<LocationDto>(queryString)
                        .ToList();

            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location by Id sucessful",
                Content = result,
                Page = page,
                PerPage = perPage,
                TotalElement = result.Count
            };

        }

        public ResponseHeader SearchLocation(string keywords)
        {
            string queryString = sql.searchLocation(keywords);
            var result = baseRepository
                         .QueryString<LocationDto>(queryString)
                         .ToList();
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location by Keyword sucessful",
                Content = result,
                Page = 1,
                PerPage = int.MaxValue,
                TotalElement = result.Count
            };
        }

        public ResponseHeader SearchLocationFormLocationDto(int page, int perPage, LocationDto keyword)
        {
            var result = locationDA.SearchLocation(page, perPage, keyword);
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location by Keyword sucessful",
                Content = result.Content,
                Page = result.Page,
                PerPage = result.PerPage,
                TotalElement = result.TotalElement
            };
        }

        public ResponseHeader AddLocation(LocationDto location)
        {
            
            using var context = new SmartLockerContext();
            Address addressAdd = new Address();
            Locations locationAdd = new Locations();
            try
            {
                //add function
                if(location.LocateId <= 0)
                {
                    //add address first
                    addressAdd = new Address
                    {
                        SubDistrict = location.SubDistrict,
                        District = location.District,
                        Province = location.Province,
                        PostalCode = location.PostalCode,
                        Status = "A",
                        CreateDate = DateTime.Now
                    };
                    context.Address.Add(addressAdd);
                    context.SaveChanges();
                    Console.WriteLine("address complete");

                    //then add location
                    locationAdd = new Locations
                    {
                        LocateName = location.LocateName,
                        Longtitude = location.Longtitude,
                        Latitude = location.Latitude,
                        AddressId = addressAdd.AddressId,
                        Status = "W",
                        CreateDate = DateTime.Now,
                        AccountId = location.AccountId
                    };
                    context.Locations.Add(locationAdd);
                    context.SaveChanges();
                    Console.WriteLine("location complete");

                    return new ResponseHeader()
                    {
                        Status = "S",
                        Message = "Add Location sucessful",
                    };
                }
                //edit location form reject form location
                else
                {
                    var editedLocation = context.Locations.Where(c => c.LocateId == location.LocateId && c.Status.Equals("WE")).FirstOrDefault();
                    if(editedLocation == null)
                    {
                        throw new ArgumentException("this location is not right for this function.");
                    }
                    else
                    {
                        locationAdd = editedLocation;
                        //edit location
                        editedLocation.LocateName = location.LocateName;
                        editedLocation.Longtitude = location.Longtitude;
                        editedLocation.Latitude = location.Latitude;
                        editedLocation.Status = "W";
                        editedLocation.UpdateDate = DateTime.Now;
                        context.SaveChanges();

                        //edit address
                        var address = context.Address.Where(c => c.AddressId == editedLocation.AddressId).FirstOrDefault();
                        addressAdd = address;
                        address.SubDistrict = location.SubDistrict;
                        address.District = location.District;
                        address.Province = location.Province;
                        address.PostalCode = location.PostalCode;
                        address.UpdateDate = DateTime.Now;
                        context.SaveChanges();

                        return new ResponseHeader()
                        {
                            Status = "S",
                            Message = "Edit Location sucessful",
                        };
                    }
                }
                
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                if(location.LocateId <= 0)
                {
                    if (addressAdd.AddressId > 0)
                    {
                        context.Address.Remove(context.Address.Where(c => c.AddressId == addressAdd.AddressId).FirstOrDefault());
                        context.SaveChanges();
                    }
                    if (locationAdd.LocateId > 0)
                    {
                        context.Locations.Remove(context.Locations.Where(c => c.LocateId == locationAdd.LocateId).FirstOrDefault());
                        context.SaveChanges();
                    }
                }
                else
                {
                    var rollbackLocation = context.Locations.Where(c => c.LocateId == locationAdd.LocateId).FirstOrDefault();
                    //rollback location
                    rollbackLocation.LocateName = locationAdd.LocateName;
                    rollbackLocation.Longtitude = locationAdd.Longtitude;
                    rollbackLocation.Latitude = locationAdd.Latitude;
                    rollbackLocation.Status = locationAdd.Status;
                    rollbackLocation.UpdateDate = locationAdd.UpdateDate;
                    context.SaveChanges();

                    //rollback address
                    var address = context.Address.Where(c => c.AddressId == addressAdd.AddressId).FirstOrDefault();
                    address.SubDistrict = addressAdd.SubDistrict;
                    address.District = addressAdd.District;
                    address.Province = addressAdd.Province;
                    address.PostalCode = addressAdd.PostalCode;
                    address.UpdateDate = addressAdd.UpdateDate;
                    context.SaveChanges();
                }
                
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public ResponseHeader GetLocationAll(GetAmountDataDto getAmount)
        {
            var locationResult = locationDA.GetLocationAll(getAmount);
            return new ResponseHeader()
            {
                Status = "S",
                Message = "Get Location By successful",
                Content = locationResult.Content,
                Page = locationResult.Page,
                PerPage = locationResult.PerPage,
                TotalElement = locationResult.TotalElement,
            };
        }

        public ResponseHeader ApproveLocation(LocationDto locationDto)
        {
            using var context = new SmartLockerContext();
            var locations = context.Locations.Where(a => a.LocateId == locationDto.LocateId).FirstOrDefault();

            if (locations == null)
            {
                throw new ConfilctDataException("locations is invalid");
            }

            try
            {
                if (locations.Status == "W" && locationDto.Status == "approve")
                {
                    var address = context.Address.Where(a => a.AddressId == locations.AddressId).FirstOrDefault();
                    address.Status = "A";
                    locations.ApproveDate = DateTime.Now;
                    locations.Status = "A";
                    context.SaveChanges();
                    return new ResponseHeader("S", "Approve location successful");
                }
                else if (locations.Status == "W" && locationDto.Status == "rejectEdit")
                {
                    var address = context.Address.Where(a => a.AddressId == locations.AddressId).FirstOrDefault();
                    address.Status = "A";
                    locations.ApproveDate = DateTime.Now;
                    locations.Status = "WE";
                    locations.Remark = locationDto.Remark;
                    context.SaveChanges();
                    return new ResponseHeader("S", "Reject to edited location successful");
                }
                else if (locations.Status == "W" && locationDto.Status == "reject")
                {
                    var address = context.Address.Where(a => a.AddressId == locations.AddressId).FirstOrDefault();
                    address.Status = "I";
                    locations.ApproveDate = DateTime.Now;
                    locations.Status = "R";
                    locations.Remark = locationDto.Remark;
                    context.SaveChanges();
                    return new ResponseHeader("S", "Reject to delete location successful");
                }
                else
                {
                    return new ResponseHeader("F", "Approve or Reject location Unsuccessful");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}