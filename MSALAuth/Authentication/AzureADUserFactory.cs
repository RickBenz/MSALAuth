using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using MSALAuth.Services;
using System.Security.Claims;

namespace MSALAuth.Authentication
{
    public class AzureADUserFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly AdminService _adminService;

        public AzureADUserFactory(IAccessTokenProviderAccessor tokenProviderAccessor,
                                  IServiceProvider serviceProvider,
                                  AdminService adminService) : base(tokenProviderAccessor)
        {
            _serviceProvider = serviceProvider;
            _adminService = adminService;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            Console.WriteLine("---- CREATE USER ASYNC CALLED -----");
            Console.WriteLine($"-- DateTime: {DateTime.Now.ToString()} --");

            string userLoginName = string.Empty;
            string employeeNumber = string.Empty;
            var initialUser = await base.CreateUserAsync(account, options);

            try
            {
                if (account?.AdditionalProperties == null
                    || !account.AdditionalProperties.TryGetValue("sub", out var userId)
                    || string.IsNullOrEmpty(userId?.ToString()))
                {
                    Console.WriteLine("------ User is not authenticated yet, skipping CreateUserAsync... -------");
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }

                if (initialUser.Identity!.IsAuthenticated)
                {
                    var userIdentity = (ClaimsIdentity)initialUser.Identity;
                    var claim = userIdentity.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");
                    string userPrinipleName = claim != null ? claim.Value.ToString() : string.Empty;
                    int loginNameIndex = userPrinipleName.LastIndexOf("@");
                    userLoginName = userPrinipleName.Substring(0, loginNameIndex);

                    if (!string.IsNullOrEmpty(userLoginName))
                    {
                        var accessTokenResult = await TokenProvider.RequestAccessToken();

                        if (accessTokenResult.TryGetToken(out var accessToken))
                        {
                            Console.WriteLine($"--- Access token available: {accessToken}", accessToken.Value ?? "No token value available");
                            Console.WriteLine($"**** Token expiry time:", accessToken.Expires.ToString());
                        }
                        else
                        {
                            Console.WriteLine("--- Access token not available, MSAL might attempt refresh?");
                        }

                        // Normally the start of getting additional User Information for GlobalUser Service
                        // example Graph info, user info from ERP, etc..
                        var breakTest = await _adminService.GetAnyDataFromClientApi();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- EXCEPTION - CreateUserAsync: {ex.Message}");
            }

            return initialUser;
        }
    }
}
