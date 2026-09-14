using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Web.Models.Mapping;

public static class WebErpMappingBootstrapper
{
	public static void Initialize()
	{
		ErpMappingBootstrapper.Initialize();
		StrategyRegistry.Register(new JTokenToAppStrategy());
		StrategyRegistry.Register(new JTokenToErpPageStrategy());
		StrategyRegistry.Register(new ErpPageToEntityRecordStrategy());
		StrategyRegistry.Register(new DataRowToPageBodyNodeStrategy());
		StrategyRegistry.Register(new DataRowToPageDataSourceStrategy());
		StrategyRegistry.Register(new JTokenToSitemapAreaStrategy());
		StrategyRegistry.Register(new JTokenToSitemapGroupStrategy());
		StrategyRegistry.Register(new JTokenToSitemapNodeStrategy());
		StrategyRegistry.Register(new DataRowToSitemapNodeStrategy());
		StrategyRegistry.Register(new ErrorModelToValidationErrorStrategy());
	}
}
