using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Project.DataSource
{
	public class MonthSelectOptions : CodeDataSource
	{
		public MonthSelectOptions() : base()
		{
			Id = new Guid("BD83B38B-0211-4AAB-9049-97E9E2847C57");
			Name = "WvProjectMonthSelectOptions";
			Description = "List of all months with month number as value";
			ResultModel = "List<SelectOption>";

			Parameters.Add(new DataSourceParameter { Name = "culture", Type = "text", Value = "en" });
		}

		public override object Execute(Dictionary<string, object> arguments)
		{
			var result = new List<SelectOption>();
			var cultureCode = (string)arguments["culture"];
			if (string.IsNullOrWhiteSpace(cultureCode)) {
				cultureCode = "en";
			}
			for (int i = 1; i < 13; i++)
			{
				var monthName = new DateTime(2015, i, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture(cultureCode));
				result.Add(new SelectOption(i.ToString(), monthName));
			}


			return result;
		}
	}
}
