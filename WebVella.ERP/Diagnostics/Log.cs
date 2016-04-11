using Microsoft.Extensions.Logging;

namespace WebVella.ERP.Diagnostics
{
	public class Log
	{
		private static readonly ILogger logger;

		static Log()
		{

			var factory = new LoggerFactory();
			factory.AddConsole(minLevel: LogLevel.Verbose);
			logger = factory.CreateLogger("diagnostics");
		}

		public static void LogMessage( string message )
		{
			logger.LogInformation(message);
		}

	}
}
