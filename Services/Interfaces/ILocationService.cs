using System.Collections.Generic;
using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Entities;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;

namespace smartlocker.software.api.Services.Interfaces
{
    public interface ILocationService
    {
        ResponseHeader GetLocation();
        ResponseHeader GetLocationById(int id);
        ResponseHeader GetLocationByAccountId(int page, int perPage, int id);
        ResponseHeader GetLocationAll(GetAmountDataDto getAmount);
        ResponseHeader ApproveLocation(LocationDto locationDto);
        ResponseHeader SearchLocation(string keywords);
        ResponseHeader AddLocation(LocationDto location);
        ResponseHeader SearchLocationFormLocationDto(int page, int perPage, LocationDto keyword);
    }
}