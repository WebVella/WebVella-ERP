using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebVella.Erp.Api.Models
{
	internal class ValidationUtility
	{
		private const string NAME_VALIDATION_PATTERN = @"^[a-z](?!.*__)[a-z0-9_]*[a-z0-9]$";
		private const string VIEW_NAME_VALIDATION_PATTERN = @"^[a-z~](?!.*__)[a-z0-9_~]*[a-z0-9~]$";

		public static List<ErrorModel> ValidateName(string name, int minLen = 2, int maxLen = 63, string key = "name")
		{
			//Apply postgres name validation
			if(maxLen > 63)
				throw new Exception("Name length cannot be more then 63 symbols");

			if (!string.IsNullOrEmpty(name))
				name = name.Trim();

			if (maxLen <= 0)
				throw new ArgumentException("maxLen<=0");

			if (minLen > maxLen)
				throw new ArgumentException("minLen > maxLen");

			List<ErrorModel> errors = new List<ErrorModel>();
			if (string.IsNullOrWhiteSpace(name))
			{
				errors.Add(new ErrorModel(key, name, "Name is required!"));
				return errors;
			}

			if (name.Length < minLen)
				errors.Add(new ErrorModel(key, name, string.Format("The Name must be at least {0} characters long!", minLen)));

			if (name.Length > maxLen)
				errors.Add(new ErrorModel(key, name, string.Format("The length of Name must be less or equal than {0} characters!", maxLen)));

			Match match = Regex.Match(name, NAME_VALIDATION_PATTERN);
			if (!match.Success || match.Value != name.Trim())
				errors.Add(new ErrorModel(key, name, "Name can only contains underscores and lowercase alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

			return errors;
		}

		public static List<ErrorModel> ValidateViewName(string name, int minLen = 2, int maxLen = 200, string key = "name")
		{
			if (!string.IsNullOrEmpty(name))
				name = name.Trim();

			if (maxLen <= 0)
				throw new ArgumentException("maxLen<=0");

			if (minLen > maxLen)
				throw new ArgumentException("minLen > maxLen");

			List<ErrorModel> errors = new List<ErrorModel>();
			if (string.IsNullOrWhiteSpace(name))
			{
				errors.Add(new ErrorModel(key, name, "Name is required!"));
				return errors;
			}

			if (name.Length < minLen)
				errors.Add(new ErrorModel(key, name, string.Format("The Name must be at least {0} characters long!", minLen)));

			if (name.Length > maxLen)
				errors.Add(new ErrorModel(key, name, string.Format("The length of Name must be less or equal than {0} characters!", maxLen)));

			Match match = Regex.Match(name, VIEW_NAME_VALIDATION_PATTERN);
			if (!match.Success || match.Value != name.Trim())
				errors.Add(new ErrorModel(key, name, "Name can only contains underscores and lowercase alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

			return errors;
		}

		public static List<ErrorModel> ValidateLabel(string label, int minLen = 1, int maxLen = 200, string key = "label")
		{
			if (!string.IsNullOrEmpty(label))
				label = label.Trim();

			if (maxLen <= 0)
				throw new ArgumentException("maxLen<=0");

			if (minLen > maxLen)
				throw new ArgumentException("minLen > maxLen");

			List<ErrorModel> errors = new List<ErrorModel>();
			if (string.IsNullOrWhiteSpace(label))
			{
				errors.Add(new ErrorModel(key, label, "Label is required!"));
				return errors;
			}

			if (label.Length < minLen)
				errors.Add(new ErrorModel(key, label, string.Format("The Label must be at least {0} characters long!", minLen)));

			if (label.Length > maxLen)
				errors.Add(new ErrorModel(key, label, string.Format("The length of Label must be less or equal than {0} characters!", maxLen)));

			return errors;
		}

		public static List<ErrorModel> ValidateLabelPlural(string label, int minLen = 1, int maxLen = 200)
		{
			if (!string.IsNullOrEmpty(label))
				label = label.Trim();

			if (maxLen <= 0)
				throw new ArgumentException("maxLen<=0");

			if (minLen > maxLen)
				throw new ArgumentException("minLen > maxLen");

			List<ErrorModel> errors = new List<ErrorModel>();
			if (string.IsNullOrWhiteSpace(label))
			{
				errors.Add(new ErrorModel("labelPlural", label, "Plural label is required!"));
				return errors;
			}

			if (label.Length < minLen)
				errors.Add(new ErrorModel("labelPlural", label, string.Format("The Plural label must be at least {0} characters long!", minLen)));

			if (label.Length > maxLen)
				errors.Add(new ErrorModel("labelPlural", label, string.Format("The length of Plural label must be less or equal than {0} characters!", maxLen)));

			return errors;
		}
	}
}
