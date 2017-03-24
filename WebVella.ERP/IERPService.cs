using WebVella.ERP.Storage;

namespace WebVella.ERP
{
    public interface IErpService
    {
        void InitializeSystemEntities();
		void InitializeBackgroundJobs();
		void StartBackgroundJobProcess();
	}
}