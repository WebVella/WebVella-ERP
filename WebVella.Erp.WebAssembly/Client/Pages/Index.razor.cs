using Microsoft.AspNetCore.Components;
using WebVella.Erp.WebAssembly.ApiService;

namespace WebVella.Erp.WebAssembly.Pages;

public partial class Index : ComponentBase
{

    [Inject] private IAuthenticationService _authService { get; set; }
    [Inject] private IApiService _apiService { get; set; }

    private bool _isAuthenticated = false;
    private WvUser _user = null;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isAuthenticated = await _authService.HasTokenAsync();
            _user = await _apiService.GetCurrentUserAsync();
            await InvokeAsync(StateHasChanged);
        }
    }
    private async Task _login()
    {
        _isAuthenticated = await _authService.LoginAsync(new LoginModel { Email = "erp@webvella.com", Password = "erp" });
    }
    private async Task _logout()
    {
        await _authService.LogoutAsync();
        _isAuthenticated = false;
    }
}
