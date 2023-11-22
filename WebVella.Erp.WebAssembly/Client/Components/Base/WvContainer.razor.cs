namespace WebVella.Erp.WebAssembly.Components;

public partial class WvContainer : ComponentBase
{
    [Inject] protected NavigationManager Navigator { get; set; }

    [Parameter] public ComponentState State { get; set; } = ComponentState.Hidden;

    [Parameter] public RenderFragment WvContent { get; set; }

    [Parameter] public RenderFragment WvLoading { get; set; }

    [Parameter] public string ErrorMessage { get; set; }
    [Parameter] public bool IsWrappInErrorBoundry { get; set; } = true;

}

