using System;

namespace WebVella.Erp.Jobs
{
	public class JobAttribute : Attribute
	{
		private Guid id;
		private string name;
		private JobPriority defaultPriority;
		bool allowSingleInstance;

		public JobAttribute(string id, string name, bool allowSingleInstance = false, JobPriority defaultPriority = JobPriority.Low )
		{
			this.id = new Guid(id);
			this.name = name;
			this.defaultPriority = defaultPriority;
			this.allowSingleInstance = allowSingleInstance;
		}

		public virtual Guid Id
		{
			get { return id; }
		}

		public virtual string Name
		{
			get { return name; }
		}

		public virtual JobPriority DefaultPriority
		{
			get { return defaultPriority; }
			set { defaultPriority = value; }
		}

		public virtual bool AllowSingleInstance
		{
			get { return allowSingleInstance; }
			set { allowSingleInstance = value; }
		}
	}
}
