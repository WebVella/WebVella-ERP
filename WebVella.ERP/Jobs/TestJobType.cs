using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebVella.ERP.Jobs
{
	public class TestJobType
	{
		public void Run(JobContext context)
		{
			Debug.WriteLine($"Started: {DateTime.Now}");

			Thread.Sleep(2000);

			Debug.WriteLine($"Compleated: {DateTime.Now}");
		}
	}
}
