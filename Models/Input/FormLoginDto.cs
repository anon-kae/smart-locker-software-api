﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLocker.Software.Backend.Models.Input 
{ 
    public class FormLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string RoleUser { get; set; }
    }
}
