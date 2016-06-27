using System;
using System.Collections.Generic;

namespace WebVella.ERP.Web.Services
{
	public class RequestService : IRequestService, IDisposable
	{
		List<IDisposable> objectsToDispose { get; set; }

		public RequestService()
		{
			objectsToDispose = new List<IDisposable>();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (IDisposable dispObj in objectsToDispose)
					dispObj.Dispose();
			}
		}

		public void AddObjectToDispose(IDisposable obj)
		{
			if (obj == null)
				return;
			if (objectsToDispose.Contains(obj))
				return;
			objectsToDispose.Add(obj);
		}
	}
}