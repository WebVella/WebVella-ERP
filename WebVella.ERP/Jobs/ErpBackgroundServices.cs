using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WebVella.Erp.Jobs
{
    public class ErpJobScheduleService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ScheduleManager.Current.ProcessSchedulesAsync(stoppingToken);
            await Task.FromResult(0);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await Task.FromResult(0);
        }
    }

    public class ErpJobProcessService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            JobManager.Current.ProcessJobsAsync(stoppingToken);
            await Task.FromResult(0);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await Task.FromResult(0);
        }
    }
}
