using Microsoft.AspNetCore.Components;

namespace WebVella.Erp.WebAssembly.Pages;

public partial class Index : ComponentBase
{

	[Inject] private IAuthenticationService _authService{ get; set; }

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var result = await _authService.LoginAsync(new LoginModel { Email = "erp@webvella.com", Password = "erp" });
		}
	}
}
