using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace WebVella.Erp.WebAssembly.Services;

public interface ITokenManagerService
{
    Task<string> GetTokenAsync();
}

public class TokenManagerService : ITokenManagerService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    public TokenManagerService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    private bool ValidateTokenExpiration(string token)
    {
        JwtSecurityToken jwtSecurityToken = null;
        try
        {
            jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
        catch
        {
            return false;
        }

        if (jwtSecurityToken is null)
            return false;

        List<Claim> claims = jwtSecurityToken.Claims.ToList();
        if (claims?.Count == 0)
            return false;

        string expirationSeconds = claims.Where(_ => _.Type.ToLower() == "exp").Select(_ => _.Value).FirstOrDefault();
        if (string.IsNullOrEmpty(expirationSeconds))
            return false;

        string tokenResfreshAfterString = claims.Where(_ => _.Type.ToLower() == "token_refresh_after").Select(_ => _.Value).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(tokenResfreshAfterString))
            return false;

        DateTime tokenResfreshAfter = DateTime.FromBinary(long.Parse(tokenResfreshAfterString));
        bool isValid = tokenResfreshAfter >= DateTime.UtcNow;

        return isValid;
    }

    private async Task<string> RefreshTokenEndPoint(JwtToken tokenModel)
    {
        JwtSecurityToken jwtSecurityToken = null;
        try
        {
            jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenModel.Token);
        }
        catch
        {
            return string.Empty;
        }

        if (jwtSecurityToken is null)
            return string.Empty;

        List<Claim> claims = jwtSecurityToken.Claims.ToList();
        string expirationSeconds = claims.Where(_ => _.Type.ToLower() == "exp").Select(_ => _.Value).FirstOrDefault();
        if (string.IsNullOrEmpty(expirationSeconds))
            return string.Empty;

        //if token is expired then we don't do refresh request because it will fail with ExpiredException
        //in that case we just return empty string and an ApiTokenException will be thrown when
        //authenticatated http client is requested
        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expirationSeconds));
        if (expirationDate < DateTime.UtcNow)
            return string.Empty;

        AuthResponse authResponse = await _httpClient.PostAndReadAsJsonAsync<JwtToken, AuthResponse>("api/v3/en_US/auth/jwt/token/refresh", tokenModel);
        var token = authResponse.Object?.ToString();
        if (string.IsNullOrWhiteSpace(token))
        {
            await _localStorageService.RemoveItemAsync("token");
            return string.Empty;
        }
        await _localStorageService.SetItemAsync("token", token);
        return token;
    }

    public async Task<string> GetTokenAsync()
    {
        string token = await _localStorageService.GetItemAsync<string>("token");
        if (string.IsNullOrWhiteSpace(token))
            return string.Empty;

        if (ValidateTokenExpiration(token))
            return token;

        JwtToken tokenModel = new JwtToken { Token = token };
        return await RefreshTokenEndPoint(tokenModel);
    }
}

