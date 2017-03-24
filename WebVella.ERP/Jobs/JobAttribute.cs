using System;

namespace WebVella.ERP.Jobs
{
	public class JobAttribute : Attribute
	{
		private Guid id;
		private string name;
		private JobPriority defaultPriority;

		public JobAttribute(string id, string name)
		{
			this.id = new Guid(id);
			this.name = name;
			this.defaultPriority = JobPriority.Low;
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
	}
}
