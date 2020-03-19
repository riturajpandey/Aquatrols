using System;
using System.Collections.Generic;
using System.Text;

namespace Aquatrols.Models
{
    public class SignUpResponseEntity
    {
        public string operationStatus { get; set; }
        public string operationMessage { get; set; }
        public string token { get; set; }
    }
}
