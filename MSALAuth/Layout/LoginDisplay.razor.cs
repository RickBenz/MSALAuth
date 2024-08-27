using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace MSALAuth.Layout
{
    public partial class LoginDisplay
    {

        [Inject] private NavigationManager Navigation { get; set; }

        [Inject] private SignOutSessionStateManager SignOutManager { get; set; }

        public void Login()
        {
            Navigation.NavigateTo("authentication/login", true);
        }

        public async Task Logout()
        {
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }

    }
}
