namespace WebVella.Erp.WebAssembly.Services;

public interface IConfigurationService
{
	string ApiUrl { get; }
	string ServerUrl { get; }
}

public class ConfigurationService : IConfigurationService
{
	public bool EnablePerformanceCounters { get; }
	public string ServerUrl { get; }
	public string ApiUrl { get { return $"{ServerUrl}api/"; } }

	private IConfiguration configuration;

	public ConfigurationService(IConfiguration config)
	{
		configuration = config;
		ServerUrl = configuration["ServerUrl"];
	}
}