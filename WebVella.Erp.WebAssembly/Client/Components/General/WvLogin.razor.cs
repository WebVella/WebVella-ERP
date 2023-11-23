namespace WebVella.Erp.WebAssembly.Components;

public partial class WvLogin : WvBaseComponent
{
    public LoginModel _form = new();
    private string _error = "";
    private Dictionary<string, List<string>> _errorDictionary = null;
    private string _returnUrl = "";

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _returnUrl = NavigatorExt.GetStringFromQuery(Navigator, WasmConstants.ReturnUrlQuery);
        State = ComponentState.Content;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (await ApiService.HasTokenAsync())
            {
                var returnUrl = NavigatorExt.GetStringFromQuery(Navigator, WasmConstants.ReturnUrlQuery, null);
                if (returnUrl is not null) Navigator.NavigateTo(returnUrl);
                else Navigator.NavigateTo("/");
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    public async Task _loginBtnClick()
    {
        try
        {
            _error = "";
            _errorDictionary = null;

            var result = await ApiService.LoginAsync(new LoginModel { Email = _form.Email, Password = _form.Password });

            if (!result)
                throw new ValidationException("Грешно потребителско име или парола");

           // var user = await ApiService.GetCurrentUserAsync();

            if (!String.IsNullOrWhiteSpace(_returnUrl))
                Navigator.NavigateTo(_returnUrl);
            else
                Navigator.NavigateTo("/");
        }
        catch (Exception ex)
        {
            _error = "Невалидни данни";
            //_errorDictionary = await ExceptionExt.Notify(ex, "_onSubmitHandler", this.GetType().Name,
            //    NotificationService, ApiService, null, "", true);
        }
    }

}

