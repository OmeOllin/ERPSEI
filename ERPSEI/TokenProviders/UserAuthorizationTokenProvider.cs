using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ERPSEI.TokenProviders
{
    public class UserAuthorizationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {

        public UserAuthorizationTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<UserAuthorizationTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger) 
        { 
        }
    }

    public class UserAuthorizationTokenProviderOptions : DataProtectionTokenProviderOptions
    {

    }
}
