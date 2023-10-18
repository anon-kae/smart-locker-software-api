using smartlocker.software.api.Models;
using SmartLocker.Software.Backend.Models;
using SmartLocker.Software.Backend.Models.Input;
using SmartLocker.Software.Backend.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Services.DataAccess.Interface
{
    public interface ILocationDA
    {
        PageModel<LocationDto> GetLocationAll(GetAmountDataDto getAmount);
        PageModel<LocationDto> SearchLocation(int page, int perPage, LocationDto keyword);
    }
}
