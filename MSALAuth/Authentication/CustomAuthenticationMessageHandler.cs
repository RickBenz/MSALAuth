namespace MSALAuth.Authentication
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

    public class CustomAuthenticationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthenticationMessageHandler(IAccessTokenProvider provider,
                                                  NavigationManager navigationManager,
                                                  IConfiguration configuration) : base(provider, navigationManager)
        {
            var authUrls = configuration.GetSection("AuthorizedUrls").Get<List<string>>() ?? [];
            var scopes = configuration.GetValue<string>("Scopes:MyAPIScope") ?? string.Empty;
            ConfigureHandler(authorizedUrls: authUrls, scopes: [scopes]);
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("------ CustomAuthenationMessageHandler Involked ------");
            Console.WriteLine("------------------------------------------------------");
        }
    }
}
