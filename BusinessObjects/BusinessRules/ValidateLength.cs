﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjects.BusinessRules
{
    
    // length validation rule. 
    // length must be between given min and max values
    
    public class ValidateLength : BusinessRule
    {
        private readonly int _min;
        private readonly int _max;

        public ValidateLength(string propertyName, int min, int max)
            : base(propertyName)
        {
            _min = min;
            _max = max;

            Error = propertyName + " must be between " + _min + " and " + _max + " characters long.";
        }

        public ValidateLength(string propertyName, string errorMessage, int min, int max)
            : this(propertyName, min, max)
        {
            Error = errorMessage;
        }

        public override bool Validate(BusinessObject businessObject)
        {
            int length = GetPropertyValue(businessObject).ToString().Length;
            return length >= _min && length <= _max;
        }
    }
}
