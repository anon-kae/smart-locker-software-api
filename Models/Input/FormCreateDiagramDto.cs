using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input
{
    public class FormCreateDiagramDto
    {
        public int FormId { get; set; }
        public List<LockerRoomDto> LockerRoomList { get; set; }
    }
}
