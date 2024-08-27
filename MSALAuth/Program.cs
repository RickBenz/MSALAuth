using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MSALAuth.Authentication;
using MSALAuth.Services;
using MudBlazor.Services;
using static MSALAuth.Constants;

namespace MSALAuth
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<CustomAuthenticationMessageHandler>();

            builder.Services.AddHttpClient(APIType.MainAPI,
                client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseOptions:MyApi:BaseUrl")))
                .AddHttpMessageHandler<CustomAuthenticationMessageHandler>();

            builder.Services.AddScoped<AdminService>();
            
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("BaseOptions:AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode = "redirect";
            }).AddAccountClaimsPrincipalFactory<AzureADUserFactory>();

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.ClearAfterNavigation = true;
            });

            await builder.Build().RunAsync();
        }
    }
}
