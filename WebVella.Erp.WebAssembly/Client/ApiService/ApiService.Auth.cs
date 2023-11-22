using Blazored.LocalStorage;

namespace WebVella.Erp.WebAssembly.ApiService;

public partial interface IApiService
{
    ValueTask<bool> HasTokenAsync();
    ValueTask<bool> LoginAsync(LoginModel model);
    ValueTask<bool> LogoutAsync();
}

public partial class ApiService : IApiService
{
    public async ValueTask<bool> HasTokenAsync()
    {
        return await _authenticationService.HasTokenAsync();
    }

    public async ValueTask<bool> LoginAsync(LoginModel model)
    {
        return await _authenticationService.LoginAsync(model);
    }
    public async ValueTask<bool> LogoutAsync()
    {
        return await _authenticationService.LogoutAsync();
    }

}