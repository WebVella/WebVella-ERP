using System;
using WebVella.Erp.Api;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Jobs;
using WebVella.Erp.Plugins.SDK.Services;

namespace WebVella.Erp.Plugins.SDK.Jobs
{
	[Job("99D9A8BB-31E6-4436-B0C2-20BD6AA23786", "Clear job and error logs job", true, JobPriority.Medium)]
	public class ClearJobAndErrorLogsJob : ErpJob
	{
		public override void Execute(JobContext context)
		{
            using (SecurityContext.OpenSystemScope())
            {
                try
                {
                    new LogService().ClearJobAndErrorLogs();
                }
                catch (Exception ex)
                {
                    new Log().Create(LogType.Error, "ClearJobAndErrorLogsJob", ex);
                }
            }
            
		}
	}
}
