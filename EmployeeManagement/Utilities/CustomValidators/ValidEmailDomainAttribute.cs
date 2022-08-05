using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Utilities.CustomValidators
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        public string AllowedDomain { get; }

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            AllowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            var domain = Convert.ToString(value).Split('@').ElementAt(1);

            return domain.ToLower().Equals(AllowedDomain.ToLower());
        }

    }
}
