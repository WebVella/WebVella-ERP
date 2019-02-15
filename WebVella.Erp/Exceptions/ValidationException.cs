using System;
using System.Collections.Generic;

namespace WebVella.Erp.Exceptions
{
	public class ValidationException : Exception
	{
		public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

		public new string Message { get; set; } = "";

		public ValidationException() : this(null, null)
		{
		}

		public ValidationException(string message) : this(message, null)
		{
		}

		public ValidationException(string message = null, Exception inner = null) : base(message, inner)
		{
			Message = message;
			Errors = new List<ValidationError>();
		}

		public void AddError(string fieldName, string message, long index = 0)
		{
			if (string.IsNullOrWhiteSpace(Message))
				Message = message;

			Errors.Add(new ValidationError(fieldName, message, false, index));
		}

		public void CheckAndThrow()
		{
			if (Errors != null && Errors.Count > 0)
				throw this;
		}
	}
}