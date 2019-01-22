using System;
using System.Collections.Generic;

namespace WebVella.Erp.Eql
{
	public class EqlException : Exception
	{


		public List<EqlError> Errors { get; private set; } = new List<EqlError>();

		public EqlException(string message) : base(string.IsNullOrWhiteSpace(message) ? "One or more Eql errors occurred." : message)
		{
			Errors.Add(new EqlError { Message = message });
		}

		public EqlException(EqlError error) : base(error?.Message ?? "One or more Eql errors occurred.")
		{
			Errors.Add(error);
		}

		public EqlException(List<EqlError> errors) : base("One or more Eql errors occurred.")
		{
			if (errors != null)
				Errors.AddRange(errors);
		}
	}
}
