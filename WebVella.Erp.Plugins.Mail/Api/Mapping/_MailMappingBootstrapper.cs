using WebVella.Erp.Api.Models.Mapping;

namespace WebVella.Erp.Plugins.Mail.Api.Mapping;

public static class MailMappingBootstrapper
{
	public static void Initialize()
	{
		StrategyRegistry.Register(new EntityRecordToEmailStrategy());
		StrategyRegistry.Register(new EmailToEntityRecordStrategy());
		StrategyRegistry.Register(new EntityRecordToSmtpServiceStrategy());
		StrategyRegistry.Register(new SmtpServiceToEntityRecordStrategy());
	}
}
