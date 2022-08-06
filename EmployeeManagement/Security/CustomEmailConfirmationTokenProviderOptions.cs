using Microsoft.AspNetCore.Identity;
using System;

namespace EmployeeManagement.Security
{
    public class CustomEmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public CustomEmailConfirmationTokenProviderOptions()
        {
            TokenLifespan = TimeSpan.FromDays(2);
        }
    }
}
