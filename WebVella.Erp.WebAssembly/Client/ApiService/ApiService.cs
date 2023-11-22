using Blazored.LocalStorage;

namespace WebVella.Erp.WebAssembly.ApiService;

public partial interface IApiService { }

public partial class ApiService : IApiService
{
    private readonly string apiProjectRoot = "/api/v3.0/p/project/";
    private readonly HttpClient _httpClient;
    private readonly ITokenManagerService _tokenManagerService;
    private readonly AuthenticationStateProvider _customAuthenticationProvider;
    private readonly ILocalStorageService _localStorageService;
    private readonly NavigationManager _navManager;
    private readonly IAuthenticationService _authenticationService;

    public ApiService(
        HttpClient httpClient,
        ITokenManagerService tokenManagerService,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider customAuthenticationProvider,
        NavigationManager navManager,
        IAuthenticationService authenticationService
        )
    {
        _httpClient = httpClient;
        _tokenManagerService = tokenManagerService;
        _localStorageService = localStorageService;
        _customAuthenticationProvider = customAuthenticationProvider;
        _navManager = navManager;
        _authenticationService = authenticationService;
    }

}