using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjects.BusinessRules
{
    
    // IP Address validation rule
    
    public class ValidateIpAddress : ValidateRegex
    {
        // Match IP Address
        public ValidateIpAddress(string propertyName) :
            base(propertyName, @"^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$")
        {
            Error = propertyName + " is not a valid IP Address";
        }

        public ValidateIpAddress(string propertyName, string errorMessage) :
            this(propertyName)
        {
            Error = errorMessage;
        }
    }
}
