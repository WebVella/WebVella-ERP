using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebVella.Erp.WebAssembly.Services;

public class CustomAuthenticationProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorageService;
    private readonly AuthenticationState anonymous;

    public CustomAuthenticationProvider(ILocalStorageService localStorageService)
    {
        anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity() { }));
        this.localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        string tokenString = await localStorageService.GetItemAsync<string>("token");
        if (string.IsNullOrEmpty(tokenString))
            return anonymous;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
        var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt"));
        var loginUser = new AuthenticationState(userClaimPrincipal);
        return loginUser;
    }

    public void Notify()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
