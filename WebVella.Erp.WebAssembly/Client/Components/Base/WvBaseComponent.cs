using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
//using WebVella.CEQ;

namespace WebVella.Erp.WebAssembly.Components;

public class WvBaseComponent : ComponentBase, IAsyncDisposable
{
    [Inject] protected IJSRuntime JSRuntimeSrv { get; set; }
    [Inject] protected NavigationManager Navigator { get; set; }
    [Inject] protected IApiService ApiService { get; set; }
    [Inject] protected IConfigurationService ConfigurationService { get; set; }
    [Parameter]
    public Guid ComponentId { get; set; } = Guid.NewGuid();

    [Parameter]
    public CultureInfo Culture { get; set; } = WasmConstants.Culture;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnknownParameters { get; set; } = new Dictionary<string, object>();

    [CascadingParameter(Name = "AppState")]
    protected AppState AppState { get; set; }

    protected ComponentState State { get; set; } = ComponentState.Loading;
    protected string ErrorMessage { get; set; } = "";

    public Task QueueInvokeAsync(Action action) { return base.InvokeAsync(action); }

    public bool IsDisposed { get; private set; } = false;
    public bool IsRenderLockEnabled { get; private set; } = false;
    public Guid CurrentRenderLock { get; private set; } = Guid.Empty;
    public Guid OldRenderLock { get; private set; } = Guid.Empty;

#pragma warning disable 1998
    public virtual async ValueTask DisposeAsync()
    {
        IsDisposed = true;
    }
#pragma warning restore 1998

    protected void EnableRenderLock()
    {
        IsRenderLockEnabled = true;
    }

    protected void DisableRenderLock()
    {
        IsRenderLockEnabled = false;
    }

    protected void RegenRenderLock()
    {
        CurrentRenderLock = Guid.NewGuid();
    }


    protected override bool ShouldRender()
    {
        if (!IsRenderLockEnabled)
        {
            return true;
        }

        if (CurrentRenderLock == OldRenderLock)
            return false;

        OldRenderLock = CurrentRenderLock;
        return true;
    }

}
