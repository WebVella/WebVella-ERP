namespace WebVella.Erp.WebAssembly.Components
{
	public partial class WvContainerStates : ComponentBase
	{
		[Parameter] public ComponentState State { get; set; } = ComponentState.Loading;
		[Parameter] public RenderFragment LoadingFragment { get; set; }
		[Parameter] public RenderFragment ContentFragment { get; set; }
		[Parameter] public RenderFragment ErrorFragment { get; set; }

		private RenderFragment GetComponentStateFragment()
		{
			return State switch
			{
				ComponentState.Loading => LoadingFragment,
				ComponentState.Content => ContentFragment,
				ComponentState.Error => ErrorFragment,
				ComponentState.Hidden => null,
				_=> ErrorFragment
			};
		}
	}
}
