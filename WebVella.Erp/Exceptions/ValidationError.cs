using System;

namespace WebVella.Erp.Exceptions
{
	public class ValidationError
	{
		/// <summary>
		/// Specifies index of the element if, validation is on enumerable
		/// </summary>
		public long Index { get; set; }

		/// <summary>
		/// Specifies property name validation is associated to
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// Specifies details about error
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Specifies if the error is system and should be logged as system error
		/// </summary>
		public bool IsSystem { get; set; }

		/// <summary>
		/// Creates ValidationError object
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="message"></param>
		/// <param name="index"></param>
		public ValidationError(string fieldName, string message, bool isSystem = false, long index = 0)
		{
			if (index < 0)
				throw new ArgumentException("index");

			//if (string.IsNullOrWhiteSpace(fieldName))
			//	throw new ArgumentException("fieldName");

			if (string.IsNullOrWhiteSpace(message))
				throw new ArgumentException("message");

			PropertyName = fieldName?.ToLowerInvariant();
			Message = message;
			Index = index;
			IsSystem = isSystem;
		}
	}
}
