using System;
using System.Collections.Generic;
using System.Text;

namespace Aquatrols.Models
{
    class LoginResponseEntity
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string token { get; set; }
        public string isApproved { get; set; }
        public bool? isActive { get; set; }
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
    }
}
