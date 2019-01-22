namespace WebVella.Erp.Jobs
{
	public abstract class ErpJob
    {
		public virtual void Execute(JobContext context)
		{
		}
    }
}
