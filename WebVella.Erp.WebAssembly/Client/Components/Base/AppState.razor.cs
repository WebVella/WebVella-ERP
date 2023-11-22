using Microsoft.AspNetCore.Components.Routing;
using System.Text.RegularExpressions;

namespace WebVella.Erp.WebAssembly.Components;

public partial class AppState : ComponentBase, IAsyncDisposable
{
	#region << Params and Injects >>
	[Parameter]
	public RenderFragment ChildContent { get; set; }

	[Inject] protected IJSRuntime JSRuntimeSrv { get; set; }

	[Inject] protected NavigationManager Navigator { get; set; }

	[Inject] protected IApiService ApiService { get; set; }

	[Inject] protected IConfigurationService ConfigurationService { get; set; }

	public bool IsDisposed { get; private set; } = false;

	public Guid ComponentId { get; set; } = Guid.NewGuid();

	private DotNetObjectReference<AppState> _objectRef;

	#endregion

	#region << Public props >>

	public Guid SessionId { get; private set; } = Guid.NewGuid();
	public WvUser User { get; private set; } = null;
	#endregion


	#region << Private props >>
	private bool _shouldRender = false;

	private string _errorMessage = "";

	private bool _shouldRerender = true;
	#endregion

	#region << Life cycle >>
	public Task QueueInvokeAsync(Action action)
	{
		return base.InvokeAsync(action);
	}

#pragma warning disable 1998
	public async ValueTask DisposeAsync()
	{
		_objectRef?.Dispose();
		Navigator.LocationChanged -= handleLocationChanged;
		IsDisposed = true;
	}

#pragma warning restore 1998
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_objectRef = DotNetObjectReference.Create((AppState)this);
			User = await ApiService.GetCurrentUserAsync();

			if (User == null)
			{
				_errorMessage = "Няма логнат потребител";
				return;
			}
			Navigator.LocationChanged += handleLocationChanged;
			_shouldRender = true;
			await InvokeAsync(StateHasChanged);
			_shouldRerender = false;
		}
	}

	protected override bool ShouldRender() => _shouldRerender; //this component should never rerender as it is caused by the location change event callback
	#endregion

	#region << Nav >>
	private void handleLocationChanged(object sender, LocationChangedEventArgs e)
	{
		base.InvokeAsync(async () =>
		{
			//await SetSearchBarVisibile(false,this);
			await Task.Delay(2);  // wait for blazor to populate route parameters
			var newLocation = Navigator.Uri;

			_shouldRerender = true;
			await InvokeAsync(StateHasChanged);
			_shouldRerender = false;
		});
	}

	#endregion



}
