using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Security
{
    public class CustomConfirmedEmailTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomConfirmedEmailTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<CustomEmailConfirmationTokenProviderOptions> customEmailConfirmationTokenProviderOptions, ILogger<CustomConfirmedEmailTokenProvider<TUser>> logger) : base(dataProtectionProvider, customEmailConfirmationTokenProviderOptions, logger)
        {

        }
    }
}
